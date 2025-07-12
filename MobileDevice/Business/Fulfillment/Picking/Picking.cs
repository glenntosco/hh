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

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Picking
{
    [ViewController("main.picking")]
    public class Picking : ProductScanController
    {
        public override string Title => "Picking";

        private LocationLookup _fromBinLpnLookupDetails;

        private PickTicketLookup _pickTicket;
        private PickReservation _pick;

        private Func<Task> _lastFunc;
        private ToteLookup _tote;

        private Button _short;
        private Button _skip;
        private Button _printTote;

        private bool _autoWavePickTicket;

        protected override async Task Init()
        {
            _lastFunc = Init;
            _short = View.RemoveToolbar(_short);
            _skip = View.RemoveToolbar(_skip);
            _printTote = View.RemoveToolbar(_printTote);
            _pick = null;

            var configs = (await Singleton<Web>.Instance.PostInvokeAsync<List<ConfigEntry>>("data/configs", new List<string>
            {
                nameof(ConfigConstants.Business_Fulfillment_Handheld_AutoWavePickTicketOnScan),
            })).ToDictionary(c => c.Name, c => c.BoolValue);
            _autoWavePickTicket = configs[nameof(ConfigConstants.Business_Fulfillment_Handheld_AutoWavePickTicketOnScan)];

            await AskPickTicket();
            if(!_pickTicket.EnforceCartonizationPicking)
                _printTote ??= View.AddToolbar("Print tote", PrintTote);

            await View.PushMessage($@"{Lang.Translate($"{(_pickTicket.IsWarehouseTransfer ? "Transfer" : "Pick ticket")} [{_pickTicket.PickTicketNumber}]")}
{Lang.Translate($"To [{_pickTicket.CustomerName}]")}
{Lang.Translate($"Picks [{_pickTicket.RemainingPicks.Count}]")}
{Lang.Translate($"Units [{_pickTicket.RemainingPicks.Sum(c => c.QuantityToPick)}]")}
{Lang.Translate("Instructions:")} {_pickTicket.PickInstructions}", Init, false);

            await PickingCycle();
        }

        private async Task AskPickTicket()
        {
            _lastFunc = AskPickTicket;
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);
            var allowedStates = new[]
            {
                PickTicketState.Waved,
                PickTicketState.BeingPicked
            };
            await LoopUntilGood(async () =>
            {
                _pickTicket = await PickTicketLookup();

                if (_pickTicket.PickTicketState == PickTicketState.ReadyToPick)
                {
                    if (_autoWavePickTicket)
                    {
                        await Singleton<Web>.Instance.PostInvokeAsync("api/PickTicketApi/AutoWave", new List<Guid> {_pickTicket.Id});
                        _pickTicket = await PickTicketLookup(_pickTicket.Id);
                    }
                    else
                        throw new ExceptionLocalized($"Cannot pick [{_pickTicket.PickTicketNumber}] - Pick ticket needs to be waved");
                }

                if (!allowedStates.Contains(_pickTicket.PickTicketState))
                    throw new ExceptionLocalized($"Cannot pick [{_pickTicket.PickTicketNumber}] - invalid status [{_pickTicket.PickTicketState}]");
            }, AskPickTicket);
        }

        protected async Task PickingCycle()
        {
            _lastFunc = PickingCycle;
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

            var nextBin = _pickTicket.RemainingPicks.OrderBy(c => c.PickSequence).ThenBy(c => c.Product.Sku).Select(c => c.BinCode).FirstOrDefault();
            if (nextBin == null)
            {
                await View.PushMessage("No more picks!");
                View.InactivateMessages();
                await Init();
                return;
            }

            var pickBatch = _pickTicket.RemainingPicks.Where(c => c.BinCode == nextBin).ToList();
            _pick = pickBatch.OrderBy(c => c.PickSequence).ThenBy(c => c.Product.Sku).ThenBy(c => c.BigText).First();
            if (_pick.FullPalletPick)
            {
                await View.PushMessage($"Full pallet pick {_pick.GetPickBin()}");
                await FullPalletAskLpn();
                return;
            }

            if (Singleton<Context>.Instance.AllowPickTicketPickShort || Singleton<Context>.Instance.UserType == UserType.Administrator)
                _short ??= View.AddToolbar("Short", Short);

            if (Singleton<Context>.Instance.AllowPickTicketPickSkip)
                _skip ??= View.AddToolbar("Skip", Skip);

            var message = $"{Lang.Translate($"From {_pick.GetPickBin()}")}\n{Lang.Translate($"Sku [{_pick.Product.Sku}]")}\n";

            if (!_pick.Product.IsPacksizeControlled && !_pick.Product.IsLotControlled && !_pick.Product.IsExpiryControlled)
                message += $"{Lang.Translate($"Quantity to pick [{_pick.QuantityToPick}]")}\n";
            else
            {
                string lot = null, expiry = null, pck;
                if (_pick.Product.IsLotControlled)
                    lot = $"{_pick.LotNumber}";
                if (_pick.Product.IsExpiryControlled)
                    expiry = $"{_pick.ExpiryString}";
                if (_pick.Product.IsPacksizeControlled)
                    pck = Lang.Translate($"[{_pick.QuantityToPick}] pack(s) of [x{_pick.Packsize}]");
                else
                    pck = Lang.Translate($"[{_pick.QuantityToPick}] units");

                var str = string.Join(" - ", new[] { lot, expiry, pck }.Where(c => c != null));
                message += $"{str}\n";
            }

            if(_pickTicket.EnforceCartonizationPicking && !string.IsNullOrWhiteSpace(_pick.BigText))
                message += $"To carton [{_pick.BigText}]\n";

            message += _pick.Product.Description;

            if (!string.IsNullOrWhiteSpace(_pick.Product.ImageUrl))
                await View.PushThumbnailMessage(message, _pick.Product.ImageUrl, null, false);
            else
                await View.PushMessage(message, null, false);
            await AskFromBinLpn();
        }

        //Regular picking
        private async Task AskFromBinLpn()
        {
            _lastFunc = AskFromBinLpn;
            await LoopUntilGood(async () =>
            {
                _fromBinLpnLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
                if (_fromBinLpnLookupDetails.Id != _pick.BinId && _fromBinLpnLookupDetails.LocationCode != _pick.Lpn)
                    throw new ExceptionLocalized($"Invalid bin, {_pick.GetPickBin()} expected");
            }, AskFromBinLpn);
            await AskProductOp();
        }

        protected override async Task AskProductOp()
        {
            _lastFunc = AskProductOp;
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProductOp, _pickTicket.ClientId);
                if (ProdDetails.Id != _pick.Product.Id)
                    throw new ExceptionLocalized($"Invalid product, [{_pick.Product.Sku}] expected");

                ProdOperation ??= new ProductOperation();
                ProdOperation.ProductId = ProdDetails.Id;
                ProdOperation.PacksizeId = ProdDetails.PacksizeId;

                if (ProdDetails.IsPacksizeControlled)
                {
                    if (_pick.PacksizeId != ProdOperation.PacksizeId)
                        throw new ExceptionLocalized($"Invalid packsize [{ProdDetails.PacksizeName}]");
                }

                if (ProdDetails.IsLotControlled)
                    await AskLot();

                if (ProdDetails.IsExpiryControlled)
                    await AskExpiry();
                
            }, AskProductOp);

            if (ProdDetails.IsSerialControlled)
                await AskTote();
            else 
                await AskQuantity();
        }

        protected override async Task AskLot()
        {
            _lastFunc = AskLot;
            await LoopUntilGood(async () =>
            {
                ProdOperation.LotNumber = await PromptLot(AskLot);
                if (_pick.LotNumber != ProdOperation.LotNumber)
                    throw new ExceptionLocalized($"Invalid Lot [{ProdOperation.LotNumber}], [{_pick.LotNumber}] expected");
            }, AskLot);
        }

        protected override async Task AskExpiry()
        {
            _lastFunc = AskExpiry;
            await LoopUntilGood(async () =>
            {
                ProdOperation.Expiry = await PromptExpiry(AskExpiry);
                if (_pick.ExpiryString != ProdOperation.Expiry)
                    throw new ExceptionLocalized($"Invalid Expiry [{ProdOperation.Expiry}], [{_pick.ExpiryString}] expected");
            }, AskExpiry);
        }

        protected override async Task AskQuantity()
        {
            _lastFunc = AskQuantity;
            await LoopUntilGood(async () =>
            {
                ProdOperation.Quantity = await PromptQuantity(AskQuantity, ProdDetails.UnitOfMeasure?.ToString());
                if (ProdOperation.Quantity <= 0)
                    throw new ExceptionLocalized("Quantity must be positive");
                if (ProdOperation.Quantity > _pick.QuantityToPick && !ProdDetails.IsDecimalControlled)
                    throw new ExceptionLocalized($"Invalid Qty. [{_pick.QuantityToPick}] expected");
            }, AskQuantity);
            await AskTote();
        }

        protected override Func<Task> ProductReady => AskTote;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => PickingCycle;

        protected async Task AskTote()
        {
            _lastFunc = AskTote;
            await LoopUntilGood(async () =>
            {
                _tote = await ToteLookup(ProductReady, _pick.BigText);
                if (_pickTicket.EnforceCartonizationPicking && _tote.BigText != _pick.BigText)
                    throw new ExceptionLocalized($"Invalid tote [{_tote.BigText}], Expected [{_pick.BigText}]");
                if (_pickTicket.Totes.All(c => c.Sscc18Code != _tote.Sscc18Code))
                    throw new ExceptionLocalized($"Invalid tote [{_tote.Sscc18Code}]");
            }, ProductReady);

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await Process();
        }

        protected override Task AskSerial()
        {
            _lastFunc = AskSerial;
            return base.AskSerial();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/fulfillment/Pick?reservationId={_pick.Id}&toteId={_tote.Id}";
                if (_pick.DetailId != null)
                    url += $"&reservationDetailId={_pick.DetailId}";
                var auditRecord = await Singleton<Web>.Instance.PostInvokeAsync<AuditRec>(url, ProdOperation);
                if (auditRecord.IsFromBinEmpty == true && auditRecord.FromLpnId == null && Singleton<Context>.Instance.EmptyBinPrompt)
                {
                    var confirm = await View.PromptBool("Bin empty?", "Yes", "No");
                    if (!confirm)
                    {
                        var id = auditRecord.FromBinId;
                        if (id != null)
                            await Singleton<Web>.Instance.PostInvokeAsync($"api/BinApi/MarkCountRequired", new List<Guid> {id.Value});
                    }
                }

                _pick.QuantityToPick -= ProdOperation.Quantity;

                View.InactivateMessages();
                if (_pick.QuantityToPick <= 0)
                {
                    _pickTicket.RemainingPicks.Remove(_pick);
                    _pick = null;
                    ProdOperation = null;
                    await View.PushMessage("Picked!");
                    await PickingCycle();
                }
                else
                {
                    await View.PushMessage($"Picked, [{_pick.QuantityToPick}] remaining");
                    if (ProdDetails.IsSerialControlled)
                        await AskSerial();
                    else
                        await AskQuantity();
                }
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await ProductReady();
            }
        }

        //Full pallet pick
        protected async Task FullPalletAskLpn()
        {
            _lastFunc = FullPalletAskLpn;
            var first = _pick;
            await LoopUntilGood(async () =>
            {
                var fullPallet = await LocationLookup(FullPalletAskLpn, "Scan LPN pick...");
                if (!fullPallet.IsLpn || fullPallet.LocationCode != first.Lpn)
                    throw new ExceptionLocalized($"Invalid LPN, [{first.Lpn}] expected");
            }, FullPalletAskLpn);
            await FullPalletAskTote();
        }

        protected async Task FullPalletAskTote()
        {
            _lastFunc = FullPalletAskTote;
            await LoopUntilGood(async () =>
            {
                _tote = await ToteLookup(FullPalletAskTote);
                if (_pickTicket.Totes.All(c => c.Sscc18Code != _tote.Sscc18Code))
                    throw new ExceptionLocalized($"Invalid tote [{_tote.Sscc18Code}]");
            }, FullPalletAskTote);
            await FullPalletProcess();
        }

        protected async Task FullPalletProcess()
        {
            try
            {
                var first = _pick;
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/fulfillment/FullPalletPick?lpnId={first.LpnId}&toteId={_tote.Id}");

                var lpnPicks = _pickTicket.RemainingPicks.Where(c => c.LpnId == first.LpnId && c.FullPalletPick).ToList();
                lpnPicks.ForEach(c => _pickTicket.RemainingPicks.Remove(c));
                View.InactivateMessages();
                await View.PushMessage($"[{first.Lpn}] picked!");
                await PickingCycle();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, FullPalletProcess);
                await FullPalletAskTote();
            }
        }

        //Toolbar buttons
        private async Task Short()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/fulfillment/Short?reservationId={_pick.Id}");
                _pickTicket.RemainingPicks.Remove(_pick);
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
                _pickTicket.RemainingPicks.Remove(_pick);
                View.InactivateMessages();
                await View.PushMessage("Skipped!");
                await PickingCycle();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Skip);
            }
        }

        private async Task PrintTote()
        {
            try
            {
                var cartonSizes = await Singleton<Web>.Instance.GetInvokeAsync<List<IdName>>($"hh/lookup/GetCartonSizes");
                var url = $"hh/fulfillment/PrintTote?pickTicketId={_pick.PickTicketId}";
                if (cartonSizes.Any())
                {
                    if (cartonSizes.Count == 1)
                        url += $"&cartonSizeId={cartonSizes.First().Id}";
                    else
                    {
                        var name = await View.PromptPicker("Carton size", cartonSizes.Select(c => c.Name).OrderBy(c => c).ToList());
                        url += $"&cartonSizeId={cartonSizes.First(c => c.Name == name).Id}";
                    }
                }

                var tote = await Singleton<Web>.Instance.GetInvokeAsync<ToteLookup>(url);
                _pickTicket.Totes.Add(tote);
                await View.PushMessage($@"[{tote.Sscc18Code}] generated");

                if (cartonSizes.Count > 1)
                {
                    if (_lastFunc != null)
                        await _lastFunc();
                    else
                        await PickingCycle();
                }
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, PrintTote);
            }
        }
    }
}