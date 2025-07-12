using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.PoReceiving
{
    [ViewController("main.printProductBarcode")]
    public class PrintProductBarcode : ProductScanController
    {
        public override string Title => "Product barcode";

        protected override async Task Init()
        {
            ProdDetails = null;
            ProdOperation = null;
            await AskProductOp();
        }

        protected override Func<Task> ProductReady => AskReference;
        protected override Func<Task> SerialReady => AskReference;
        protected override Func<Task> NoMoreSerials => Init;

        protected override async Task AskProductOp()
        {
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

            ProdDetails = await ProductLookup(AskProductOp);
            ProdOperation ??= new ProductOperation();
            ProdOperation.ProductId = ProdDetails.Id;
            ProdOperation.PacksizeId = ProdDetails.PacksizeId;

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await AskQuantity();
        }

        protected override async Task AskQuantity()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Quantity = await PromptQuantity(AskQuantity);
                if (ProdOperation.Quantity <= 0)
                    throw new ExceptionLocalized("Quantity must be positive");
            }, AskQuantity);
            await ProductReady();
        }

        protected virtual async Task AskReference()
        {
            ProdOperation.ReferenceCode = await View.PromptString("Enter/Scan reference...");
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                View.InactivateMessages();
                await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/PrintProductLabels", ProdOperation);
                await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
        }
    }
}