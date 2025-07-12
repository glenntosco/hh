using System;
using System.IO;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Products
{
    [ViewController("main.productPhotoCollect")]
    public class ProductPhotoCollect : ScanScreenController
    {
        public override string Title => "Product photo";

        private ProductDetails _prodDetails;
        
        protected override async Task Init()
        {
            _prodDetails = await ProductLookup(Init);
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
                await Singleton<Web>.Instance.UploadStream($"hh/floor/UploadProductPhoto?productId={_prodDetails.Id}", new MemoryStream(bytes));
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