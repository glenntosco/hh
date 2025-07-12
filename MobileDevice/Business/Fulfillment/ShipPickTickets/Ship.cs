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
    [ViewController("main.recordDriver")]
    public class Ship : ScanScreenController
    {
        public override string Title => "Ship pick ticket";

        private  PickTicketShipSettings _settings;
        private Button _picture;
        private Func<Task> _lastFunc;
        private PickTicketLookup _pickTicket;
        private byte[] _signature;
        private string _pickTicketMessage;

        protected override async Task Init()
        {
            _picture = View.RemoveToolbar(_picture);
            _lastFunc = Init;
            _pickTicket = await LoopUntilGood(async () =>
            {
                var pck = await PickTicketLookup();
                if (pck.PickTicketState != PickTicketState.Rating && pck.PickTicketState != PickTicketState.PendingDriverSignature)
                    throw new ExceptionLocalized($"PickTicket [{pck.PickTicketNumber}] invalid state [{pck.PickTicketState}]");
                await Singleton<Web>.Instance.GetInvokeAsync($"api/PickTicketApi/TestShip?key={pck.Id}");
                return pck;
            }, Init);
            
            _pickTicketMessage = $@"{_pickTicket.PickTicketNumber}
{Lang.Translate($"To [{_pickTicket.CustomerName}]")}
{Lang.Translate($"Totes [{_pickTicket.Totes.Count}]")}
{Lang.Translate($"Units [{_pickTicket.Totes.Sum(c => c.Lines.Sum(c1 => c1.PickedQuantity))}]")}";
            await View.PushMessage(_pickTicketMessage, Init, false);

            _picture = View.AddToolbar("Picture", Picture);
            _settings = new PickTicketShipSettings {PickTicketIds = new List<Guid> { _pickTicket.Id}};
            await PromptDriver();
        }

        private async Task PromptDriver()
        {
            _lastFunc = PromptDriver;
            _settings.DriverId = await View.PromptString(Lang.Translate("Driver id"));
            await View.PushMessage($"Driver id: [{_settings.DriverId}]");
            await PromptVehicle();
        }

        private async Task PromptVehicle()
        {
            _lastFunc = PromptVehicle;
            _settings.VehicleId = await View.PromptString(Lang.Translate("Vehicle id"));
            await View.PushMessage($"Vehicle id: [{_settings.VehicleId}]");
            await PromptSeal();
        }

        private async Task PromptSeal()
        {
            _lastFunc = PromptSeal;
            _settings.SealNumber = await View.PromptString(Lang.Translate("Seal number"));
            await View.PushMessage($"Seal number: [{_settings.SealNumber}]");

            if (!string.IsNullOrWhiteSpace(_pickTicket.SignUrl))
            {
                await Main.NavigateToView(null, new SignatureView
                {
                    Title = Lang.Translate("Pickup signature"),
                    Description = _pickTicketMessage,
                    OnSign = async bytes =>
                    {
                        Main.RestoreTo(View);
                        _signature = bytes;
                        if (_signature == null)
                        {
                            View.ClearMessages();
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
                if (_signature != null)
                    await View.PushImageMessage(_signature);

                if (!await View.PromptBool("Ship?", "Yes", "No"))
                    View.ClearMessages();
                else
                {
                    if (_signature != null)
                        await Singleton<Web>.Instance.PostInvokeAsync<dynamic>($"api/PickTicketApi/SignAuthenticated?key={_settings.PickTicketIds.First()}&tenant={Singleton<Context>.Instance.Tenant}", 
                            $"data:image/png;base64,{Convert.ToBase64String(_signature)}");
                    
                    await Singleton<Web>.Instance.PostInvokeAsync("hh/fulfillment/Ship", _settings);
                    
                    View.InactivateMessages();
                    await View.PushMessage("Shipped!");
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