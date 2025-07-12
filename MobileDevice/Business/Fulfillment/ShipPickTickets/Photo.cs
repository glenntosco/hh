using System;
using System.IO;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipPickTickets
{
    [ViewController("main.photoPickTicket")]
    public class Photo : ScanScreenController
    {
        public override string Title => "Photo pick ticket";

        private PickTicketLookup _pickTicket;

        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                _pickTicket = await PickTicketLookup();
                //if (_pickTicket.PickTicketState == PickTicketState.Closed)
                //    throw new ExceptionLocalized($"PickTicket [{_pickTicket.PickTicketNumber}] invalid state [{_pickTicket.PickTicketState}]");
                await View.PushMessage($"{_pickTicket.PickTicketNumber}\n" +
                                       Lang.Translate($"To [{_pickTicket.CustomerName}]"), Init, false);

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
                await Singleton<Web>.Instance.UploadStream($"hh/fulfillment/UploadPhoto?pickTicketId={_pickTicket.Id}", new MemoryStream(bytes));
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