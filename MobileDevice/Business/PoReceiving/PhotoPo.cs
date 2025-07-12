using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Receiving;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.PoReceiving
{
    [ViewController("main.photoPo")]
    public class PhotoPo : ScanScreenController
    {
        public override string Title => "Photo PO";

        private PurchaseOrder _po;

        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                var pos = await PoContainerLookup();
                if(!pos.Any())
                    throw new ExceptionLocalized($"PO cannot be found");

                _po = pos.First();
                //if (_po.PurchaseOrderState == PurchaseOrderState.Closed)
                //    throw new ExceptionLocalized($"PO [{_po.PurchaseOrderNumber}] invalid state [{_po.PurchaseOrderState}]");

                await View.PushMessage($@"{Lang.Translate($"PO [{_po.PurchaseOrderNumber}]")}
{Lang.Translate($"Vendor [{_po.VendorCompanyName}]")}
{Lang.Translate($"Lines [{_po.Lines.Count(c => c.OutstandingQuantity > 0)}]")}
{Lang.Translate($"Units [{_po.Lines.Sum(c => c.OutstandingQuantity)}]")}", Init, false);
            }, Init);
            
            await Picture();
        }

        private async Task Picture()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());
            if (photo == null)
            {
                await View.PopLastMessage();
                await Init();
                return;
            }
            
            var bytes = Utils.ReadStream(photo.GetStream());

            await View.PushImageMessage(bytes);
            if (!await View.PromptBool("Confirm", "Yes", "No"))
            {
                await View.PopLastMessage();
                await Picture();
                return;
            }

            try
            {
                await Singleton<Web>.Instance.UploadStream($"hh/receive/UploadPhoto?poId={_po.Id}", new MemoryStream(bytes));
                View.InactivateMessages();
                await View.PushMessage("Picture uploaded");
                await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message);
            }
        }
    }
}