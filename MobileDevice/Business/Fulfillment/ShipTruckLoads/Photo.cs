using System;
using System.IO;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipTruckLoads
{
    [ViewController("main.photoTruckLoad")]
    public class Photo : ScanScreenController
    {
        public override string Title => "Photo truck load";

        private TruckLoadLookup _truckLoad;

        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                _truckLoad = await TruckLoadLookup();
                if (_truckLoad.TruckLoadState == TruckLoadState.Shipped)
                    throw new ExceptionLocalized($"Truck load [{_truckLoad.TruckLoadNumber}] invalid state [{_truckLoad.TruckLoadState}]");
                var truckLoadMessage = $@"{_truckLoad.TruckLoadNumber}
{Lang.Translate($"BOL [{_truckLoad.BillOfLadingNumber}]")}
{Lang.Translate($"To [{_truckLoad.CustomerName}]")}
{Lang.Translate($"Totes [{_truckLoad.TotalTotes}]")}
{Lang.Translate($"Units [{_truckLoad.TotalUnits}]")}
{Lang.Translate($"Weight [{_truckLoad.TotalWeight:#.##}] [{_truckLoad.WeightUoM}]")}";
                await View.PushMessage(truckLoadMessage, Init, false);
            }, Init);

            if (!await View.PromptBool("Confirm?", "Yes", "No"))
            {
                View.ClearMessages();
                await Init();
            }
            else
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
                await Singleton<Web>.Instance.UploadStream($"hh/truckLoad/UploadPhoto?truckLoadId={_truckLoad.Id}", new MemoryStream(bytes));
                View.InactivateMessages();

                if (await View.PromptBool("Another picture?", "Yes", "No"))
                    await Picture(); 
                else
                    await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message);
            }
        }
    }
}