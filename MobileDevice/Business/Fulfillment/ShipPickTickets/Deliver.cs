using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipPickTickets
{
    [ViewController("main.deliver")]
    public class Deliver : ScanScreenController
    {
        public override string Title => "Deliver";

        private PickTicketShipSettings _settings;
        private Button _picture;
        private Func<Task> _lastFunc;
        private PickTicketLookup _pickTicket;

        private byte[] _signature;

        protected override async Task Init()
        {
            _signature = null;
            _picture = View.RemoveToolbar(_picture);
            _lastFunc = Init;
            _pickTicket = await PickTicketLookup();

            var message = $@"{_pickTicket.PickTicketNumber}
{Lang.Translate($"To [{_pickTicket.CustomerName}]")}
{Lang.Translate($"Totes [{_pickTicket.Totes.Count}]")}
{Lang.Translate($"Units [{_pickTicket.Totes.Sum(c => c.Lines.Sum(c1 => c1.ShippedQuantity))}]")}";

            await View.PushMessage(message, null, false);

            if (_pickTicket.PickTicketState != PickTicketState.Shipped &&
                _pickTicket.PickTicketState != PickTicketState.PendingDeliverySignature)
            {
                await View.PushError($"PickTicket [{_pickTicket.PickTicketNumber}] invalid state [{_pickTicket.PickTicketState}]");
                await Init();
                return;
            }

            _picture = View.AddToolbar("Picture", Picture);
            _settings = new PickTicketShipSettings { PickTicketIds = new List<Guid> { _pickTicket.Id } };

            if (!string.IsNullOrWhiteSpace(_pickTicket.SignUrl))
            {
                await Main.NavigateToView(null, new SignatureView
                {
                    Title = Lang.Translate("Delivery signature"),
                    Description = message,
                    OnSign = async (bytes) =>
                    {
                        Main.RestoreTo(View);
                        _signature = bytes;
                        if (_signature == null)
                        {
                            await View.PopLastMessage();
                            await Init();
                        }
                        else
                            await Process();
                    }
                });
            }
            else
                await Process();
        }

        protected async Task Process()
        {
            try
            {
                _lastFunc = Process;
                if(_signature != null)
                    await View.PushImageMessage(_signature);

                if (!await View.PromptBool("Deliver?", "Yes", "No"))
                    View.ClearMessages();
                else
                {
                    if (_signature != null)
                        await Singleton<Web>.Instance.PostInvokeAsync($"api/PickTicketApi/SignAuthenticated?key={_settings.PickTicketIds.First()}&tenant={Singleton<Context>.Instance.Tenant}",
                            $"data:image/png;base64,{Convert.ToBase64String(_signature)}");
                    else
                        await Singleton<Web>.Instance.GetInvokeAsync($"hh/fulfillment/Deliver?pickTicketId={_pickTicket.Id}");
                    View.InactivateMessages();
                    await View.PushMessage("Delivered!");
                }
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                await Init();
            }
        }

        private async Task Picture()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());
            if (photo == null)
            {
                if (_lastFunc != null)
                    await _lastFunc?.Invoke();
                return;
            }

            var bytes = Utils.ReadStream(photo.GetStream());
            await View.PushImageMessage(bytes);
            if (!await View.PromptBool("Confirm", "Yes", "No"))
            {
                await View.PopLastMessage();
                if (_lastFunc != null)
                    await _lastFunc?.Invoke();
                return;
            }

            try
            {
                await Singleton<Web>.Instance.UploadStream($"hh/fulfillment/UploadPhoto?pickTicketId={_settings.PickTicketIds.First()}", new MemoryStream(bytes));
                if (_lastFunc != null)
                    await _lastFunc?.Invoke();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message);
            }
        }
    }
}