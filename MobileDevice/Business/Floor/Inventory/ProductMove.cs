using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.productMove")]
    public class ProductMove : ProductScanController
    {
        public override string Title => "Product move";

        private LocationLookup _fromBinLookupDetails;
        private LocationLookup _toBinLookupDetails;
        
        protected override async Task Init()
        {
            if (AssignedTask != null)
            {
                var message = $@"{Lang.Translate($"Move [{AssignedTask.Details.Sku}]")}
{Lang.Translate($"From [{AssignedTask.Details.From}]")}
";
                if (AssignedTask.Details.Packsize != null)
                    message += $@"{Lang.Translate($"Packsize [x{AssignedTask.Details.Packsize}]")}
{Lang.Translate($"Packs [{AssignedTask.Details.Quantity}]")}";
                else
                    message += $@"{Lang.Translate($"Qty [{AssignedTask.Details.Quantity}]")}";

                message += $@"
{Lang.Translate($"To [{AssignedTask.Details.To}]")}";
                await View.PushMessage(message, null, false);
            }
            
            await AskFromBinLpn();
        }

        private async Task AskFromBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromBinLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
                if (AssignedTask != null && _fromBinLookupDetails.Id != AssignedTask.Details.FromId)
                    throw new ExceptionLocalized($"Invalid From [{_fromBinLookupDetails.LocationCode}], expected [{AssignedTask.Details.From}]");
            });
            
            await AskProductOp();
        }

        protected override async Task AskProductOp()
        {
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProductOp);
                if (AssignedTask != null && AssignedTask.Details.Sku != ProdDetails.Sku)
                    throw new ExceptionLocalized($"Invalid Product [{ProdDetails.Sku}], expected [{AssignedTask.Details.Sku}]");

                if (AssignedTask?.Details.Packsize != null && 
                    ProdDetails.PacksizeId != ProdDetails.Packsizes.SingleOrDefault(c => c.EachCount == AssignedTask.Details.Packsize)?.Id)
                {
                    var enteredPacksize = ProdDetails.Packsizes.Single(c => c.Id == ProdDetails.PacksizeId);
                    throw new ExceptionLocalized($"Invalid Packsize [x{enteredPacksize.EachCount}], expected [x{AssignedTask.Details.Packsize}]");
                }
            });
            
            ProdOperation ??= new ProductOperation();
            ProdOperation.ProductId = ProdDetails.Id;
            ProdOperation.PacksizeId = ProdDetails.PacksizeId;

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskToBinLpn();
            else
                await AskQuantity();
        }

        protected override async Task AskQuantity()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Quantity = await PromptQuantity(AskQuantity, ProdDetails.UnitOfMeasure?.ToString());
                if (ProdOperation.Quantity <= 0)
                    throw new ExceptionLocalized("Quantity must be positive");
                if (AssignedTask != null && ProdOperation.Quantity > AssignedTask.Details.Quantity)
                    throw new ExceptionLocalized($"Invalid Qty [{ProdOperation.Quantity}], expected [{AssignedTask.Details.Quantity}]");
            }, AskQuantity);
            await ProductReady();
        }

        protected override Func<Task> ProductReady => AskToBinLpn;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => Init;

        private async Task AskToBinLpn()
        {
            if (Singleton<Context>.Instance.IsDisplayLocationOnMove)
            {
                var prodLocations = await Singleton<Web>.Instance.GetInvokeAsync<ProductAvailability>($"hh/lookup/ProductContentsLookup?productId={ProdDetails.Id}&onlyPickable=true");
                if (prodLocations.Records.Any())
                    await View.PushMessage(prodLocations.GetOnHandQtyText(Lang.Translate), null, false);
            }
            else if (Singleton<Context>.Instance.IsSuggestPutawayReceiving)
            {
                var bins = await Singleton<Web>.Instance.PostInvokeAsync<List<string>>($"hh/lookup/SuggestPutaway?productId={ProdDetails.Id}&source=directmove", ProdOperation);
                if (bins.Any())
                    await View.PushMessage(string.Join("\n", bins), null, false);
            }

            await LoopUntilGood(async () =>
            {
                _toBinLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
                if (AssignedTask != null && _toBinLookupDetails.Id != AssignedTask.Details.ToId)
                    throw new ExceptionLocalized($"Invalid To [{_toBinLookupDetails.LocationCode}], expected [{AssignedTask.Details.To}]");
            });
            
            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/ProductMove?{_fromBinLookupDetails.QueryUrl}&{_toBinLookupDetails.QueryUrl}";
                if (AssignedTask != null)
                    url += $"&taskId={AssignedTask.Id}";
                var detls = await Singleton<Web>.Instance.PostInvokeAsync<UserTaskDetails>(url, ProdOperation);
                if (AssignedTask != null)
                    AssignedTask.Details = detls;

                View.InactivateMessages();
                if(detls != null)
                    await View.PushMessage($"Moved, [{detls.Quantity}] remaining");
                else
                    await View.PushMessage($"Moved!");

                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskFromBinLpn();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskToBinLpn();
            }
        }
    }
}