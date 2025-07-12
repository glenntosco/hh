using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Receiving;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.PoReceiving
{
    [ViewController("main.nonPoReceiving")]
    public class NonPoReceiving : ProductScanController
    {
        public override string Title => "Non-PO receiving";
        private PurchaseOrder _po;
        
        private LocationLookup _toBinLpnLookupDetails;
        protected override async Task Init()
        {
            await AskPo();
        }

        protected async Task AskPo()
        {
            _po = await LoopUntilGood(async () =>
            {
                var pos = await PoContainerLookup();
                if (pos.Count > 1)
                    throw new ExceptionLocalized($"Only one PO allowed");

                var po = pos.First();
                if (po.PurchaseOrderState != PurchaseOrderState.NotReceived &&
                    po.PurchaseOrderState != PurchaseOrderState.PartiallyReceived &&
                    po.PurchaseOrderState != PurchaseOrderState.Received)
                    throw new ExceptionLocalized($"Cannot receive {(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}], invalid state [{po.PurchaseOrderState}]");

                var message = $@"{Lang.Translate($"{(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}]")}
{Lang.Translate($"From [{po.VendorCompanyName}]")}
{Lang.Translate($"Lines [{po.Lines.Count(c => c.OutstandingQuantity > 0)}]")}";
                message += $"\n{Lang.Translate($"Units [{po.Lines.Sum(c => c.OutstandingQuantity)}]")}";

                await View.PushMessage(message, AskPo, false);
                return po;
            }, AskPo);

            if (_po == null)
                await AskPo();

            await AskProduct();
        }

        protected async Task AskProduct()
        {
            ProdDetails = await ProductLookup(AskProduct, _po.ClientId);
            ProdOperation = new ProductOperation
            {
                ProductId = ProdDetails.Id,
                PacksizeId = ProdDetails.PacksizeId
            };

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await AskQuantity();
        }

        protected override Func<Task> ProductReady => AskToBinLpn;
        protected override Func<Task> SerialReady => AskToBinLpn;
        protected override Func<Task> NoMoreSerials => AskProduct;

        private async Task AskToBinLpn()
        {
            if (Singleton<Context>.Instance.IsSuggestPutawayReceiving)
            {
                var bins = await Singleton<Web>.Instance.PostInvokeAsync<List<string>>($"hh/lookup/SuggestPutaway?productId={ProdDetails.Id}&warehouseId={_po.WarehouseId}&source=receiving", ProdOperation);
                if (bins.Any())
                    await View.PushMessage(string.Join("\n", bins), null, false);
            }

            _toBinLpnLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            ProdOperation.Reason = await ReasonCodeLookup(ProductReady, "NonPoReceive");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                if (ProdOperation.Quantity > 0)
                {
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/NonPoReceive?poId={_po.Id}&{_toBinLpnLookupDetails.QueryUrl}", ProdOperation);

                    var message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] received!");
                    if (ProdDetails.PacksizeId != null)
                        message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] pack(s) of [x{ProdDetails.EachCount}] received!");
                    await View.PushMessage(message, null, false);
                }
                View.InactivateMessages();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskProduct();
            }
        }
    }
}