using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipTruckLoads
{
    [ViewController("main.shipTruckLoad")]
    public class Ship : ScanScreenController
    {
        public override string Title => "Ship truck load";

        private Button _picture;
        private Func<Task> _lastFunc;
        private TruckLoadLookup _truckLoad;
        private string _truckLoadMessage;

        protected override async Task Init()
        {
            _picture = View.RemoveToolbar(_picture);
            _lastFunc = Init;
            _truckLoad = await LoopUntilGood(async () =>
            {
                var load = await TruckLoadLookup();
                if(load.MasterTruckLoadId != null)
                    load = await TruckLoadLookup(load.MasterTruckLoadId);
                if (load.TruckLoadState != TruckLoadState.Staging &&
                    load.TruckLoadState != TruckLoadState.PendingShipperSignature &&
                    load.TruckLoadState != TruckLoadState.PendingCarrierSignature)
                    throw new ExceptionLocalized($"TruckLoad [{load.TruckLoadNumber}] invalid state [{load.TruckLoadState}]");

                if(load.IsMasterTruckLoad)
                    await Singleton<Web>.Instance.GetInvokeAsync($"api/MasterTruckLoadApi/TestShip?key={load.Id}");
                else
                    await Singleton<Web>.Instance.GetInvokeAsync($"api/TruckLoadApi/TestShip?key={load.Id}");
                return load;
            }, Init);

            _truckLoadMessage = $@"{_truckLoad.TruckLoadNumber}
{Lang.Translate($"BOL [{_truckLoad.BillOfLadingNumber}]")}
{Lang.Translate($"To [{_truckLoad.CustomerName}]")}
{Lang.Translate($"Totes [{_truckLoad.TotalTotes}]")}
{Lang.Translate($"Units [{_truckLoad.TotalUnits}]")}
{Lang.Translate($"Weight [{_truckLoad.TotalWeight:#.##}] [{_truckLoad.WeightUoM}]")}";
            await View.PushMessage(_truckLoadMessage, Init, false);
            if (!await View.PromptBool("Confirm?", "Yes", "No"))
                await Init();
            else
                await PromptShipperSignature();
        }
        
        protected async Task PromptShipperSignature()
        {
            _lastFunc = PromptShipperSignature;
            if (_truckLoad.TruckLoadState == TruckLoadState.PendingShipperSignature)
                await Main.NavigateToView(null, new SignatureView
                {
                    Title = Lang.Translate("Shipper signature"),
                    Description = _truckLoadMessage,
                    OnSign = async bytes =>
                    {
                        Main.RestoreTo(View);
                        if (bytes == null)
                        {
                            View.ClearMessages();
                            await Init();
                        }
                        else
                        {
                            await View.PushImageMessage(bytes);
                            await Singleton<Web>.Instance.PostInvokeAsync($"api/TruckLoadApi/Sign?key={_truckLoad.Id}&tenant={Singleton<Context>.Instance.Tenant}",
                                $"data:image/png;base64,{Convert.ToBase64String(bytes)}");
                            _truckLoad = await TruckLoadLookup(_truckLoad.Id);
                            await PromptCarrierSignature();
                        }
                    }
                });
            else
                await PromptCarrierSignature();
        }

        protected async Task PromptCarrierSignature()
        {
            _lastFunc = PromptCarrierSignature;
            if (_truckLoad.TruckLoadState == TruckLoadState.PendingCarrierSignature)
                await Main.NavigateToView(null, new SignatureView
                {
                    Title = Lang.Translate("Carrier signature"),
                    Description = _truckLoadMessage,
                    OnSign = async bytes =>
                    {
                        Main.RestoreTo(View);
                        if (bytes == null)
                        {
                            View.ClearMessages();
                            await Init();
                        }
                        else
                        {
                            await View.PushImageMessage(bytes);
                            await Singleton<Web>.Instance.PostInvokeAsync($"api/TruckLoadApi/Sign?key={_truckLoad.Id}&tenant={Singleton<Context>.Instance.Tenant}",
                                $"data:image/png;base64,{Convert.ToBase64String(bytes)}");
                            _truckLoad = await TruckLoadLookup(_truckLoad.Id);
                            await Process();
                        }
                    }
                });
            else
                await Process();
        }

        protected async Task Process()
        {
            try
            {
                _lastFunc = Process;
                _picture ??= View.AddToolbar("Picture", Picture);
                if (!await View.PromptBool("Ship?", "Yes", "No"))
                    View.ClearMessages();
                else
                {
                    if(_truckLoad.IsMasterTruckLoad)
                        await Singleton<Web>.Instance.PostInvokeAsync("api/MasterTruckLoadApi/Ship", new List<Guid> {_truckLoad.Id});
                    else
                        await Singleton<Web>.Instance.PostInvokeAsync("api/TruckLoadApi/Ship", new List<Guid> { _truckLoad.Id });
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
                await Singleton<Web>.Instance.UploadStream($"hh/truckLoad/UploadPhoto?truckLoadId={_truckLoad.Id}", new MemoryStream(bytes));
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