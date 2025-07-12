using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Returns;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.RmaReceiving
{
    [ViewController("main.nonRmaReceiving")]
    public class NonRmaReceiving : ProductScanController
    {
        public override string Title => "Non-RMA receiving";
        private CustomerReturn _rma;

        private LocationLookup _toBinLpnLookupDetails;
        protected override async Task Init()
        {
            await AskRma();
        }

        protected async Task AskRma()
        {
            await LoopUntilGood(async () =>
            {
                _rma = await RmaLookup();
                if (_rma.CustomerReturnState != CustomerReturnState.NotReceived &&
                    _rma.CustomerReturnState != CustomerReturnState.PartiallyReceived &&
                    _rma.CustomerReturnState != CustomerReturnState.Received)
                    throw new ExceptionLocalized($"Cannot receive RMA [{_rma.CustomerReturnNumber}], invalid state [{_rma.CustomerReturnState}]");

                var message = $@"RMA [{_rma.CustomerReturnNumber}]
{Lang.Translate($"From [{_rma.CustomerCompanyName}]")}
{Lang.Translate($"Lines [{_rma.Lines.Count(c => c.OutstandingQuantity > 0)}]")}";
                message += $"\n{Lang.Translate($"Units [{_rma.Lines.Sum(c => c.OutstandingQuantity)}]")}";
                await View.PushMessage(message, AskRma, false);
            }, AskRma);

            if (_rma == null)
                await AskRma();

            await AskProduct();
        }

        protected async Task AskProduct()
        {
            ProdDetails = await ProductLookup(AskProduct, _rma.ClientId);
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
            if (Singleton<Context>.Instance.IsSuggestPutawayReturns)
            {
                var bins = await Singleton<Web>.Instance.PostInvokeAsync<List<string>>($"hh/lookup/SuggestPutaway?productId={ProdDetails.Id}&warehouseId={_rma.WarehouseId}&source=returns", ProdOperation);
                if (bins.Any())
                    await View.PushMessage(string.Join("\n", bins), null, false);
            }

            _toBinLpnLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            ProdOperation.Reason = await ReasonCodeLookup(ProductReady, "NonRmaReceive");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                if (ProdOperation.Quantity > 0)
                {
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/NonRmaReceive?rmaId={_rma.Id}&{_toBinLpnLookupDetails.QueryUrl}", ProdOperation);

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