using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Returns;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.RmaReceiving
{
    [ViewController("main.photoRma")]
    public class PhotoRma : ScanScreenController
    {
        public override string Title => "Photo RMA";

        private CustomerReturn _custReturn;

        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                _custReturn = await RmaLookup();
                //if (_custReturn.CustomerReturnState == CustomerReturnState.Closed)
                //    throw new ExceptionLocalized($"RMA [{_custReturn.CustomerReturnNumber}] invalid state [{_custReturn.CustomerReturnState}]");
                await View.PushMessage($@"{Lang.Translate($"RMA [{_custReturn.CustomerReturnNumber}]")}
{Lang.Translate($"Customer [{_custReturn.CustomerCompanyName}]")}
{Lang.Translate($"Lines [{_custReturn.Lines.Count(c => c.OutstandingQuantity > 0)}]")}
{Lang.Translate($"Units [{_custReturn.Lines.Sum(c => c.OutstandingQuantity)}]")}", Init, false);
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
                await Singleton<Web>.Instance.UploadStream($"hh/receive/UploadRmaPhoto?custReturnId={_custReturn.Id}", new MemoryStream(bytes));
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