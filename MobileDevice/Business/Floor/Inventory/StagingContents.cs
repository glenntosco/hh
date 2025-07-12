using System.Linq;
using System.Threading.Tasks;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.stagingContents")]
    public class StagingContents : ScanScreenController
    {
        public override string Title => "Staging contents";

        protected override async Task Init()
        {
            var binLookupDetails = await StagingContentsLookup(Init, "Scan staging bin...");

            if (!binLookupDetails.LicensePlates.Any())
                await View.PushMessage("Staging location is empty.");
            else
                await View.PushMessage(string.Join("\n", binLookupDetails.LicensePlates.OrderBy(c => c)), null, false);

            await Init();
        }
    }
}