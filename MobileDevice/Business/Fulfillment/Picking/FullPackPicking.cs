using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Picking
{
    //Will create a totemaster for EVERY unit that is picked. Ex: if I pick qty = 10, it'll create 10 tote masters and will put each prod in each box. This is compliance.
    [ViewController("main.fullPackPicking")]
    public class FullPackPicking : ProductScanController
    {
        public override string Title => "Full pack picking";

        private LocationLookup _fromBinLpnLookupDetails;
        private LocationLookup _toLpnDetails;

        private List<PickTicketLookup> _pickTickets;
        private List<PickReservation> _pickBatch = new List<PickReservation>();
        
        private Button _short;
        private Button _skip;

        protected override async Task Init()
        {
            _short = View.RemoveToolbar(_short);
            _skip = View.RemoveToolbar(_skip);
            _pickBatch = new List<PickReservation>();
            _pickTickets = new List<PickTicketLookup>();

            await AskPickTicket();
        }

        private async Task AskPickTicket()
        {
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);
            await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForcePickTicketScan)
                        key = await View.PromptScan("Scan Tag/Pick ticket...");
                    else
                        key = await View.PromptString("Scan Tag/Pick ticket...");
                }
                else
                    key = AssignedTask.ReferenceNumber;

                _pickTickets = await Singleton<Web>.Instance.GetInvokeAsync<List<PickTicketLookup>>($"hh/lookup/FullPackPickTicketLookup?key={key}");

                var pickTicketMessage = Lang.Translate(_pickTickets.Count > 1 ? $"Pick tickets [{_pickTickets.Count}]" : $"Pick ticket [{_pickTickets.First().PickTicketNumber}]");
                await View.PushMessage($@"{Lang.Translate($"Customer [{_pickTickets.First().CustomerName}]")}
{pickTicketMessage}
{Lang.Translate($"Picks [{_pickTickets.SelectMany(c => c.RemainingPicks).Count()}]")}
{Lang.Translate($"Cartons [{_pickTickets.SelectMany(c => c.RemainingPicks).Sum(c => c.QuantityToPick)}]")}", Init, false);
            }, AskPickTicket);

            await PickingCycle();
        }
        
        protected async Task PickingCycle()
        {
            try
            {
                FinishSerialButton = View.RemoveToolbar(FinishSerialButton);
                var nextSeq = _pickTickets.SelectMany(c => c.RemainingPicks)
                    .OrderBy(c => c.PickSequence)
                    .Select(c => c.PickSequence)
                    .FirstOrDefault();
                if (nextSeq == 0)
                {
                    await View.PushMessage("No more picks!");
                    View.InactivateMessages();
                    await Init();
                    return;
                }

                _pickBatch = _pickTickets
                    .SelectMany(c => c.RemainingPicks)
                    .Where(c => c.PickSequence == nextSeq)
                    .OrderBy(c => c.BinCode)
                    .ThenBy(c => c.Product.Sku)
                    .ThenByDescending(c => c.QuantityToPick)
                    .ToList();
                var first = _pickBatch.First();
                if (first.FullPalletPick)
                    throw new ExceptionLocalized($"Full pallet pick is not support for Carton picking. Use conventional picking");

                if (Singleton<Context>.Instance.AllowPickTicketPickShort || Singleton<Context>.Instance.UserType == UserType.Administrator)
                    _short ??= View.AddToolbar("Short", Short);

                if (Singleton<Context>.Instance.AllowPickTicketPickSkip)
                    _skip ??= View.AddToolbar("Skip", Skip);

                var message = $"{Lang.Translate($"From {first.GetPickBin()}")}\n{Lang.Translate($"Sku [{first.Product.Sku}]")}\n";

                _pickBatch = _pickBatch.Where(c => c.BinId == first.BinId).Where(c => c.Product.Id == first.Product.Id).ToList();

                if (_pickBatch.Count == 1 && !first.Product.IsPacksizeControlled && !first.Product.IsLotControlled && !first.Product.IsExpiryControlled)
                    message += $"{Lang.Translate($"Qty [{_pickBatch.Sum(c => c.QuantityToPick)}]")}\n";
                else
                {
                    var group = _pickBatch.GroupBy(c => $"{c.LotNumber}|{c.ExpiryString}|{c.PacksizeId}");
                    foreach (var gr in group)
                    {
                        string lot = null, expiry = null, pck = null;
                        var detl = gr.First();
                        if (detl.Product.IsLotControlled)
                            lot = $"{detl.LotNumber}";
                        if (detl.Product.IsExpiryControlled)
                            expiry = $"{detl.ExpiryString}";
                        if (detl.Product.IsPacksizeControlled)
                            pck = Lang.Translate($"[{gr.Sum(c=>c.QuantityToPick)}] pack(s) of [x{detl.Packsize}]");
                        else
                            pck = Lang.Translate($"[{gr.Sum(c=>c.QuantityToPick)}] units");

                        var str = string.Join(" - ", new[] {lot, expiry, pck}.Where(c => c != null));
                        if (!string.IsNullOrWhiteSpace(str))
                            message += $"{str}\n";
                    }
                }

                message += first.Product.Description;

                if (!string.IsNullOrWhiteSpace(_pickBatch.First().Product.ImageUrl))
                    await View.PushThumbnailMessage(message, _pickBatch.First().Product.ImageUrl, null, false);
                else
                    await View.PushMessage(message, null, false);
                await AskFromBinLpn();
            }
            catch (Exception e)
            {
                await View.PushError(e.Message);
                View.InactivateMessages();
                await Init();
            }
        }

        private async Task AskFromBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromBinLpnLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
                if (_fromBinLpnLookupDetails.Id != _pickBatch.First().BinId && _fromBinLpnLookupDetails.LocationCode != _pickBatch.First().Lpn)
                    throw new ExceptionLocalized($"Invalid bin, {_pickBatch.First().GetPickBin()} expected");
            }, AskFromBinLpn);
            await AskProductOp();
        }

        protected override async Task AskProductOp()
        {
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProductOp, _pickTickets.First().ClientId);
                if (ProdDetails.Id != _pickBatch.First().Product.Id)
                    throw new ExceptionLocalized($"Invalid product, [{_pickBatch.First().Product.Sku}] expected");

                ProdOperation ??= new ProductOperation();
                ProdOperation.ProductId = ProdDetails.Id;
                ProdOperation.PacksizeId = ProdDetails.PacksizeId;

                if (ProdDetails.IsPacksizeControlled)
                {
                    if (_pickBatch.All(c => c.PacksizeId != ProdOperation.PacksizeId))
                        throw new ExceptionLocalized($"Invalid packsize [{ProdDetails.PacksizeName}]");
                    _pickBatch = _pickBatch.Where(c => c.PacksizeId == ProdOperation.PacksizeId).ToList();
                }

                if (ProdDetails.IsLotControlled)
                    await AskLot();

                if (ProdDetails.IsExpiryControlled)
                    await AskExpiry();
                
            }, AskProductOp);

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else 
                await AskQuantity();
        }

        protected override async Task AskLot()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.LotNumber = await PromptLot(AskLot);
                if (_pickBatch.All(c => c.LotNumber != ProdOperation.LotNumber))
                    throw new ExceptionLocalized($"Invalid Lot [{ProdOperation.LotNumber}]");
                _pickBatch = _pickBatch.Where(c => c.LotNumber == ProdOperation.LotNumber).ToList();
            }, AskLot);
        }

        protected override async Task AskExpiry()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Expiry = await PromptExpiry(AskExpiry);
                if (_pickBatch.All(c => c.ExpiryString != ProdOperation.Expiry))
                    throw new ExceptionLocalized($"Invalid Expiry [{ProdOperation.Expiry}]");
                _pickBatch = _pickBatch.Where(c => c.ExpiryString == ProdOperation.Expiry).ToList();
            }, AskExpiry);
        }

        protected override async Task AskQuantity()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Quantity = await PromptQuantity(AskQuantity, ProdDetails.UnitOfMeasure?.ToString());
                if (ProdOperation.Quantity <= 0)
                    throw new ExceptionLocalized("Quantity must be positive");
                var expQty = _pickBatch.Sum(c => c.QuantityToPick);
                if (ProdOperation.Quantity > expQty && !ProdDetails.IsDecimalControlled)
                    throw new ExceptionLocalized($"Invalid Qty. [{expQty}] expected");
            }, AskQuantity);
            await AskLpn();
        }

        protected override Func<Task> ProductReady => AskLpn;
        protected override Func<Task> SerialReady => AskLpn;
        protected override Func<Task> NoMoreSerials => PickingCycle;

        private async Task AskLpn()
        {
            _toLpnDetails = await LoopUntilGood(async () =>
            {
                var lpn = await LocationLookup(AskLpn, "Scan LPN...", BinDirection.In, true);
                if (lpn?.IsLpn == false)
                    throw new ExceptionLocalized("Cannot scan bin, LPN expected");
                return lpn;
            }, AskLpn);
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var pickedQty = ProdOperation.Quantity;
                var total = ProdOperation.Quantity;
                var current = 0m;
                View.InactivateMessages();

                while (pickedQty > 0)
                {
                    var _pick = _pickBatch
                        .OrderBy(c => c.QuantityToPick)
                        .First();
                    var url = $"hh/fulfillment/FullPackPick?reservationId={_pick.Id}";
                    if (_toLpnDetails != null)
                        url += $"&{_toLpnDetails.QueryUrl}";
                    if (_pick.DetailId != null)
                        url += $"&reservationDetailId={_pick.DetailId}";

                    if (_pick.QuantityToPick < pickedQty)
                        ProdOperation.Quantity = _pick.QuantityToPick;
                    
                    current += _pick.QuantityToPick;
                    await View.PushMessage($"Please wait [{current}/{total}]");
                    await Singleton<Web>.Instance.PostInvokeAsync(url, ProdOperation);
                    await View.PopLastMessage();

                    pickedQty -= ProdOperation.Quantity;
                    _pick.QuantityToPick -= ProdOperation.Quantity;
                    if (_pick.QuantityToPick <= 0)
                    {
                        var pickTicket = _pickTickets.Single(c => c.Id == _pick.PickTicketId);
                        pickTicket.RemainingPicks.Remove(_pick);
                        _pickBatch.Remove(_pick);
                    }
                }
                View.InactivateMessages();

                if (pickedQty < _pickBatch.Sum(c => c.QuantityToPick))
                {
                    await View.PushMessage($"Picked, [{_pickBatch.Sum(c => c.QuantityToPick)}] remaining");
                    if (ProdDetails.IsSerialControlled)
                        await AskSerial();
                    else
                        await AskQuantity();
                }
                else
                {
                    ProdOperation = null;
                    await View.PushMessage("Picked!");
                    await PickingCycle();
                }
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await ProductReady();
            }
        }

        //Toolbar buttons
        private async Task Short()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/fulfillment/Short?reservationId={_pickBatch.First().Id}");
                foreach (var pick in _pickBatch)
                {
                    var pickTicket = _pickTickets.Single(c => c.Id == pick.PickTicketId);
                    pickTicket.RemainingPicks.Remove(pick);
                }
                View.InactivateMessages();
                await View.PushMessage("Shorted!");
                await PickingCycle();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Short);
            }
        }

        private async Task Skip()
        {
            try
            {
                //await Singleton<Web>.Instance.GetInvokeAsync($"hh/fulfillment/Short?reservationId={_pickBatch.First().Id}");
                foreach (var pick in _pickBatch)
                {
                    var pickTicket = _pickTickets.Single(c => c.Id == pick.PickTicketId);
                    pickTicket.RemainingPicks.Remove(pick);
                }
                View.InactivateMessages();
                await View.PushMessage("Skipped!");
                await PickingCycle();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Skip);
            }
        }
    }
}