using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.Floor.Bulk
{
    [ViewController("main.letdownProductByBin")]
    public class LetdownProductByBin : LetdownProduct
    {
        public override LetdownSequence LetdownSequence => LetdownSequence.ByBin;
        public override string Title => "Product letdown by Bin";
    }

    [ViewController("main.letdownProductBySku")]
    public class LetdownProductBySku : LetdownProduct
    {
        public override LetdownSequence LetdownSequence => LetdownSequence.BySku;
        public override string Title => "Product letdown by product";
    }

    public abstract class LetdownProduct : ProductScanController
    {
        private LocationLookup _fromLpnLookupDetails;
        private LocationLookup _toBinLookupDetails;

        private Button _skipButton;
        private int _skip;

        private ProductLetdownRequest _letdown;

        private string _key;

        public abstract LetdownSequence LetdownSequence { get; }

        protected override async Task Init()
        {
            _skip = 0;
            _skipButton = View.RemoveToolbar(_skipButton);

            await LoopUntilGood(async () =>
            {
                await AskLetdownTicket();
            });
        }

        protected async Task AskLetdownTicket()
        {
            if (AssignedTask == null)
                _key = await View.PromptString("Letdown ticket");
            else
                _key = AssignedTask.ReferenceNumber;
            AssignedTask = null;

            await CheckLetdownTicket(_key);
        }

        private async Task CheckLetdownTicket(string key)
        {
            _skipButton ??= View.AddToolbar("Skip", Skip);

            if (_letdown != null && _skip >= _letdown.TotalMoves)
                _skip = 0;

            var url = $"hh/lookup/ProductLetdownLookup?lookupType={LetdownSequence}&skip={_skip}";
            if (!string.IsNullOrWhiteSpace(key))
                url += $"&key={key}";
            _letdown = await Singleton<Web>.Instance.GetInvokeAsync<ProductLetdownRequest>(url);
            
            if (_letdown == null)
            {
                if (string.IsNullOrWhiteSpace(key))
                    await View.PushMessage("No pending letdowns");
                else
                    await View.PushMessage($"No pending letdowns for [{key}]");
                await Init();
                return;
            }

            var message = $@"[{_letdown.LicensePlate}] @ [{_letdown.BinCode}]
{Lang.Translate($"Sku [{_letdown.Sku}]")}";
            if (!_letdown.Details.Any())
                message += $@"
{Lang.Translate($"Move [{_letdown.Quantity}] units")}";
            else
            {
                foreach (var detl in _letdown.Details)
                {

                    if (detl.PacksizeEachCount != null)
                        message += $"\n{Lang.Translate($"Move [{(int) detl.Quantity}] pack(s) of [x{detl.PacksizeEachCount}]")}";
                    else
                        message += $"\n{Lang.Translate($"Move [{detl.Quantity}] units")}";

                    if (!string.IsNullOrWhiteSpace(detl.LotNumber))
                        message += $"\n{Lang.Translate($"Lot [{detl.LotNumber}]")}";

                    if (!string.IsNullOrWhiteSpace(detl.Expiry))
                        message += $"\n{Lang.Translate($"Exp [{detl.Expiry}]")}";
                }
            }

            if (_letdown.SuggestedBins.Any())
                message += $"\n{Lang.Translate($"To [{string.Join(",", _letdown.SuggestedBins)}]")}";

            if (string.IsNullOrWhiteSpace(_letdown.ImageUrl))
                await View.PushMessage(message, null, false);
            else
                await View.PushThumbnailMessage(message, _letdown.ImageUrl, null, false);

            await AskLpn();
        }

        private async Task AskLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromLpnLookupDetails = await LocationLookup(AskLpn, "Scan LPN...");
                if (!_fromLpnLookupDetails.IsLpn)
                    throw new ExceptionLocalized("Cannot scan bin, LPN expected");
                if(_letdown.LicensePlate != _fromLpnLookupDetails.LpnCode)
                    throw new ExceptionLocalized($"Invalid LPN, [{_letdown.LicensePlate}] expected");
            }, AskLpn);

            await AskProductOp();
        }

        protected override async Task AskProductOp()
        {
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);
            await LoopUntilGood(async () =>
            {
                var prodDetails = await ProductLookup(() => AskProductOp(false));
                if (prodDetails.Id != _letdown.ProductId)
                    throw new ExceptionLocalized($"Invalid product, [{_letdown.Sku}] expected");

                if (prodDetails.PacksizeId != null && _letdown.Details.All(c => c.PacksizeId != prodDetails.PacksizeId))
                    throw new ExceptionLocalized($"Invalid packsize, [{string.Join(", ", _letdown.Details.Select(c => $"x{c.PacksizeEachCount}"))}] expected");

                ProdDetails = prodDetails;
                ProdOperation = new ProductOperation
                {
                    ProductId = ProdDetails.Id,
                    PacksizeId = ProdDetails.PacksizeId
                };

                if (ProdDetails.IsLotControlled)
                    await AskLot();

                if (ProdDetails.IsExpiryControlled)
                    await AskExpiry();

                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskQuantity(false);
            }, AskProductOp);
        }

        protected override async Task AskLot()
        {
            await LoopUntilGood(async () =>
            {
                var lot = await PromptLot();
                if (_letdown.Details.All(c => c.LotNumber != lot))
                    throw new ExceptionLocalized($"Invalid lot, [{string.Join(", ", _letdown.Details.Select(c => c.LotNumber))}] expected");
                ProdOperation.LotNumber = lot;
            });
        }

        protected override async Task AskExpiry()
        {
            await LoopUntilGood(async () =>
            {
                var exp = await PromptExpiry();
                if (_letdown.Details.All(c => c.Expiry != exp))
                    throw new ExceptionLocalized($"Invalid expiry, [{string.Join(", ", _letdown.Details.Select(c => c.Expiry))}] expected");
                ProdOperation.Expiry = exp;
            });
        }

        protected override Func<Task> ProductReady => () =>
        {
            if (_letdown.Details.Any())
            {
                var detl = _letdown.Details
                    .Where(c => c.PacksizeId == ProdOperation.PacksizeId)
                    .Where(c => c.LotNumber == ProdOperation.LotNumber)
                    .SingleOrDefault(c => c.Expiry == ProdOperation.Expiry);
                if(detl == null)
                    throw new ExceptionLocalized($"Invalid letdown");
            }

            return AskToBin();
        };
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => Init;

        private async Task AskToBin()
        {
            if (Singleton<Context>.Instance.IsDisplayLocationOnMove)
            {
                var prodLocations = await Singleton<Web>.Instance.GetInvokeAsync<ProductAvailability>($"hh/lookup/ProductContentsLookup?productId={ProdDetails.Id}&take=5&onlyPickable=true");
                if (prodLocations.Records.Any())
                    await View.PushMessage(prodLocations.GetOnHandQtyText(Lang.Translate), null, false);
            }

            await LoopUntilGood(async () =>
            {
                _toBinLookupDetails = await LocationLookup(AskToBin, "Scan to bin...", BinDirection.In);
                if (_toBinLookupDetails.IsLpn)
                    throw new ExceptionLocalized("Cannot scan LPN, Bin expected");
            }, AskToBin);

            await Process();
        }
        
        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/LetdownProduct?{_fromLpnLookupDetails.QueryUrl}&{_toBinLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.PostInvokeAsync(url, ProdOperation);
                View.InactivateMessages();
                await View.PushMessage($"[{_fromLpnLookupDetails.LocationCode}] replenished [{_toBinLookupDetails?.LocationCode}]");
                await CheckLetdownTicket(_key);
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToBin();
            }
        }

        private async Task Skip()
        {
            _skip++;
            await CheckLetdownTicket(_key);
        }
    }
}