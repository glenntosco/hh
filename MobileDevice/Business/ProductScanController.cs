using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business
{
    public abstract class ProductScanController : ScanScreenController
    {
        protected ProductOperation ProdOperation;
        protected ProductDetails ProdDetails;

        protected abstract Func<Task> ProductReady { get; }
        protected abstract Func<Task> SerialReady { get; }
        protected abstract Func<Task> NoMoreSerials { get; }

        protected Button FinishSerialButton;

        protected virtual async Task AskProductOp()
        {
            await AskProductOp(false);
        }

        protected virtual async Task AskProductOp(bool allowZero)
        {
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

            ProdDetails = await ProductLookup(() => AskProductOp(allowZero));
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
                await AskQuantity(allowZero);
        }

        protected virtual async Task AskLot()
        {
            await LoopUntilGood(async () => { ProdOperation.LotNumber = await PromptLot(); });
        }

        protected virtual async Task AskExpiry()
        {
            await LoopUntilGood(async () => { ProdOperation.Expiry = await PromptExpiry(); });
        }

        protected virtual async Task AskQuantity()
        {
            await AskQuantity(false);
        }

        protected virtual async Task AskQuantity(bool allowZero)
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Quantity = await PromptQuantity(() => AskQuantity(allowZero), ProdDetails.UnitOfMeasure?.ToString());
                if (ProdOperation.Quantity < 0)
                    throw new ExceptionLocalized("Quantity cannot be negative");
                if (ProdOperation.Quantity == 0 && !allowZero)
                    throw new ExceptionLocalized("Quantity must be positive");
                await ProductReady();
            }, () => AskQuantity(allowZero));
        }

        protected virtual async Task AskSerial()
        {
            await LoopUntilGood(async () =>
            {
                if (NoMoreSerials != null)
                    FinishSerialButton ??= View.AddToolbar("No SN", async () =>
                    {
                        FinishSerialButton = View.RemoveToolbar(FinishSerialButton);
                        await NoMoreSerials();
                    });

                ProdOperation.SerialNumber = await PromptSerial(AskSerial);
                ProdOperation.Quantity = 1;
                await SerialReady();
            }, AskQuantity);
        }
    }
}