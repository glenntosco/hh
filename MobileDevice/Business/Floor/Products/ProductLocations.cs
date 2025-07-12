using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Products
{
    [ViewController("main.productLocations")]
    public class ProductLocations : ScanScreenController
    {
        public override string Title => "Product locations";

        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                var prodDetails = await ProductLookup(null);
                var prodLocations = await Singleton<Web>.Instance.GetInvokeAsync<ProductAvailability>($"hh/lookup/ProductContentsLookup?productId={prodDetails.Id}");

                if (!prodLocations.Records.Any())
                    await View.PushMessage("No product in the warehouse");
                else
                    await View.PushMessage(prodLocations.GetOnHandQtyText(Lang.Translate), null, false);
            });

            await Init();
        }
    }
}