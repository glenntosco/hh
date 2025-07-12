using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.binContents")]
    public class BinContents : ScanScreenController
    {
        public override string Title => "Bin contents";

        protected override async Task Init()
        {
            var binLookupDetails = await BinContentsLookup(Init, "Scan Bin/LPN...", true);

            if (!binLookupDetails.Contents.Any())
                await View.PushMessage("Bin empty.");
            else
            {
                foreach (var loc in binLookupDetails.Contents)
                {
                    var prod = await Singleton<Web>.Instance.GetInvokeAsync<ProductDetails>($"hh/lookup/ProductLookupById?productId={loc.ProductId}");

                    var msg = $"{Lang.Translate($"Sku: [{prod.Sku}]")}\n";
                    if (!string.IsNullOrWhiteSpace(loc.LicensePlate))
                        msg += $"{Lang.Translate($"LPN: {loc.LicensePlate}")}\n";
                    msg += $@"{Lang.Translate($"Open quantity: [{loc.OpenQuantity}]")}
{Lang.Translate($"Reserved quantity: [{loc.ReservedQuantity}]")}
{Lang.Translate($"Total quantity: [{loc.TotalQuantity}]")}
{prod.Description}
";
                    foreach (var detail in loc.BinDetails
                        .OrderByDescending(c => c.PacksizeEachCount)
                        .ThenBy(c => c.LotNumber)
                        .ThenBy(c => c.ExpiryDate)
                        .ThenBy(c => c.SerialNumber))
                    {
                        msg += "\n";
                        if (prod.IsPacksizeControlled)
                            msg += Lang.Translate($"[{detail.Quantity}] pack(s) of [x{detail.PacksizeEachCount}] ");
                        if (prod.IsLotControlled)
                            msg += Lang.Translate($"Lot [{detail.LotNumber}] ");
                        if (prod.IsExpiryControlled)
                            msg += Lang.Translate($"Expiry [{detail.ExpiryDate?.ToString("MMMM dd yyyy")}] ");
                        if (prod.IsSerialControlled)
                            msg += Lang.Translate($"SN: [{detail.SerialNumber}]");
                        if (!prod.IsPacksizeControlled && !prod.IsSerialControlled)
                            msg += $"- {detail.Quantity}";
                    }

                    if (string.IsNullOrWhiteSpace(prod.ImageUrl))
                        await View.PushMessage(msg, null, false);
                    else
                        await View.PushThumbnailMessage(msg, prod.ImageUrl, null, false);
                }
            }

            await Init();
        }
    }
}