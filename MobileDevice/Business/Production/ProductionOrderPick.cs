using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.DataTransferObjects.Dto.Production;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Production
{
    [ViewController("main.pickingProductionOrder")]
    public class ProductionOrderPick : ProductScanController
    {
        public override string Title => "Production order picking";

        private LocationLookup _fromBinLpnLookupDetails;
        private LocationLookup _toBinLookupDetails;

        private List<ProductionOrder> _subOrders;

        private PickReservation _pick = null;

        private Button _short;
        private Button _skip;

        protected override async Task Init()
        {
            var allowedStates = new[]
            {
                ProductionOrderState.BeingPicked,
                ProductionOrderState.ReadyToPick
            };

            _short = View.RemoveToolbar(_short);
            _skip = View.RemoveToolbar(_skip);

            await LoopUntilGood(async () =>
            {
                _pick = null;
                _subOrders = await ProductionOrderBatchLookup();
                foreach (var wo in _subOrders)
                {
                    if (!allowedStates.Contains(wo.ProductionOrderState))
                        throw new ExceptionLocalized($"Cannot pick [{wo.ProductionOrderNumber}], invalid status [{wo.ProductionOrderState}]");

                    await View.PushMessage($@"{Lang.Translate($"Subst order [{wo.ProductionOrderNumber}]")}
{Lang.Translate($"Picks [{wo.RemainingPicks.Count}]")}
{Lang.Translate($"Units [{wo.RemainingPicks.Sum(c => c.QuantityToPick)}]")}", null, false);
                }

                await PickingCycle();
            }, Init);
        }

        protected async Task PickingCycle()
        {
            _pick = _subOrders.SelectMany(c => c.RemainingPicks)
                .OrderBy(c => c.BinCode)
                .ThenBy(c => c.PickSequence).FirstOrDefault();
            if (_pick == null)
            {
                await View.PushMessage("No more picks!");
                View.InactivateMessages();
                await Init();
                return;
            }

            if (Singleton<Context>.Instance.AllowProdOrderPickSkip || Singleton<Context>.Instance.UserType == UserType.Administrator)
                _skip ??= View.AddToolbar("Skip", Skip);

            if (Singleton<Context>.Instance.AllowProdOrderPickShort || Singleton<Context>.Instance.UserType == UserType.Administrator)
                _short ??= View.AddToolbar("Short", Short);

            var subOrder = _subOrders.Single(c => c.Id == _pick.ProductionOrderId);
            var subMsg = _subOrders.Count > 1 ? $"{Lang.Translate($"Subst order [{subOrder.ProductionOrderNumber}]")}\n" : string.Empty;
            var message = $@"{Lang.Translate(@"Pick")}
{subMsg}{Lang.Translate($@"From {_pick.GetPickBin()}")}
{Lang.Translate($"To [{subOrder.WorkAreaBinCode}]")}
{Lang.Translate($"Sku [{_pick.Product.Sku}]")}
";

            if (!_pick.Product.IsPacksizeControlled && !_pick.Product.IsLotControlled && !_pick.Product.IsExpiryControlled)
                message += $"{Lang.Translate($"Qty [{_pick.QuantityToPick}]")}\n\n";
            else
            {
                string lot = null, expiry = null;
                if (_pick.Product.IsLotControlled)
                    lot = $"{_pick.LotNumber}";
                if (_pick.Product.IsExpiryControlled)
                    expiry = $"{_pick.ExpiryString}";
                var pck = Lang.Translate($"[{_pick.QuantityToPick}] {(!_pick.Product.IsPacksizeControlled ? "units" : $"pack(s) of [x{_pick.Packsize}]")}");

                var str = string.Join(" - ", new[] { lot, expiry, pck }.Where(c => c != null));
                if (!string.IsNullOrWhiteSpace(str))
                    message += $"{str}\n\n";
            }

            message += _pick.Product.Description;

            if (!string.IsNullOrWhiteSpace(_pick.Product.ImageUrl))
                await View.PushThumbnailMessage(message, _pick.Product.ImageUrl, null, false);
            else
                await View.PushMessage(message, null, false);
            await AskFromBinLpn();
        }

        private async Task AskFromBinLpn()
        {
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
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProductOp, _pick?.Product.ClientId);
                if (ProdDetails.Id != _pick.Product.Id)
                    throw new ExceptionLocalized($"Invalid product, [{_pick.Product.Sku}] expected");

                ProdOperation ??= new ProductOperation();
                ProdOperation.ProductId = ProdDetails.Id;
                ProdOperation.PacksizeId = ProdDetails.PacksizeId;

                if (ProdDetails.IsPacksizeControlled)
                {
                    if (_pick.PacksizeId != ProdOperation.PacksizeId)
                        throw new ExceptionLocalized($"Invalid packsize [{ProdDetails.PacksizeName}], [x{_pick.Packsize}] expected");
                }

                if (ProdDetails.IsLotControlled)
                    await AskLot();

                if (ProdDetails.IsExpiryControlled)
                    await AskExpiry();

            }, AskProductOp);

            if (ProdDetails.IsSerialControlled)
                await AskWorkArea();
            else
                await AskQuantity();
        }

        protected override async Task AskLot()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.LotNumber = await PromptLot(AskLot);
                if (_pick.LotNumber != ProdOperation.LotNumber)
                    throw new ExceptionLocalized($"Invalid Lot [{ProdOperation.LotNumber}], [{_pick.LotNumber}] expected");
            }, AskLot);
        }

        protected override async Task AskExpiry()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Expiry = await PromptExpiry(AskExpiry);
                if (_pick.ExpiryString != ProdOperation.Expiry)
                    throw new ExceptionLocalized($"Invalid Expiry [{ProdOperation.Expiry}], [{_pick.ExpiryString}] expected");
            }, AskExpiry);
        }

        protected override async Task AskQuantity()
        {
            await LoopUntilGood(async () =>
            {
                ProdOperation.Quantity = await PromptQuantity(AskQuantity, ProdDetails.UnitOfMeasure?.ToString());
                if (ProdOperation.Quantity <= 0)
                    throw new ExceptionLocalized("Quantity must be positive");

                if (ProdOperation.Quantity > _pick.QuantityToPick && !ProdDetails.IsDecimalControlled)
                    throw new ExceptionLocalized($"Invalid Qty. [{_pick.QuantityToPick}] expected");
            }, AskQuantity);
            await AskWorkArea();
        }

        private async Task AskWorkArea()
        {
            await LoopUntilGood(async () =>
            {
                _toBinLookupDetails = await LocationLookup(AskWorkArea, "Scan work area...", BinDirection.In);
                var subOrder = _subOrders.Single(c => c.Id == _pick.ProductionOrderId);
                if (_toBinLookupDetails.Id != subOrder.WorkAreaBinId)
                    throw new ExceptionLocalized($"Invalid work area, [{subOrder.WorkAreaBinCode}] expected");
            }, AskWorkArea);

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await Process();
        }

        protected override Func<Task> ProductReady => AskWorkArea;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => PickingCycle;

        protected async Task Process()
        {
            try
            {
                var url = $"hh/production/PickProductionOrder?reservationId={_pick.Id}";
                if (_pick.DetailId != null)
                    url += $"&reservationDetailId={_pick.DetailId}";
                await Singleton<Web>.Instance.PostInvokeAsync(url, ProdOperation);
                _pick.QuantityToPick -= ProdOperation.Quantity;
                View.InactivateMessages();
                if (_pick.QuantityToPick <= 0)
                {
                    _subOrders.Single(c => c.Id == _pick.ProductionOrderId).RemainingPicks.Remove(_pick);
                    _pick = null;
                    ProdOperation = null;
                    await View.PushMessage("Picked!");
                    await PickingCycle();
                }
                else
                {
                    await View.PushMessage($"[{ProdOperation.Quantity}] picked, [{_pick.QuantityToPick}] remaining");
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

        private async Task Skip()
        {
            try
            {
                var subOrder = _subOrders.SingleOrDefault(c => c.RemainingPicks.Contains(_pick));
                subOrder?.RemainingPicks.Remove(_pick);

                View.InactivateMessages();
                await View.PushMessage("Skipped!");
                await PickingCycle();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Skip);
            }
        }

        private async Task Short()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/production/ShortProductionOrder?reservationId={_pick.Id}");
                
                var subOrder = _subOrders.SingleOrDefault(c => c.RemainingPicks.Contains(_pick));
                subOrder?.RemainingPicks.Remove(_pick);

                View.InactivateMessages();
                await View.PushMessage("Shorted!");
                await PickingCycle();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Short);
            }
        }
    }
}