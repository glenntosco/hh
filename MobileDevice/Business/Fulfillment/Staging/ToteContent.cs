using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.toteContent")]
    public class ToteContent : ScanScreenController
    {
        public override string Title => "Tote content";

        protected override async Task Init()
        {
            var tote = await ToteLookup(Init, null, null, false);

            if (!tote.Lines.Any())
                await View.PushMessage($"Tote [{tote.Sscc18Code}] is empty.");
            else
            {
                foreach (var toteLine in tote.Lines)
                {
                    var prod = await Singleton<Web>.Instance.GetInvokeAsync<ProductDetails>($"hh/lookup/ProductLookupById?productId={toteLine.ProductId}");

                    var msg = $"{Lang.Translate($"Sku: [{prod.Sku}]")}\n";
                    msg += $@"{Lang.Translate($"Quantity: [{toteLine.PickedQuantity}]")}
{prod.Description}
";
                    foreach (var detail in toteLine.Details
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

            View.InactivateMessages();

            await Init();
        }
    }
}