using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment
{
    public abstract class DirectedDockMoveBase : ScanScreenController
    {
        private LocationLookup _foundLpn;
        private ToteLookup _foundTote;
        private LocationLookup _dockDoor;
        private List<ToteLookup> _totesToPick;
        private bool _requireDockDoorAssignment;

        public override string Title => "Tote to dock";

        protected override async Task Init()
        {
            _requireDockDoorAssignment = (await Singleton<Web>.Instance.GetInvokeAsync<ConfigEntry>($"data/config/{RequireDockDoorConfigName}")).BoolValue;
            await LoopUntilGood(async () =>
            {
                await InitChild();
                if (DockDoorId == null && _requireDockDoorAssignment)
                    throw new ExceptionLocalized($"[{ReferenceNumber}] dock door assignment is missing");
                _totesToPick = Totes.Where(c => DockDoorId != null ? c.DockDoorId != DockDoorId : c.BindId != null || c.DockDoorId != null).ToList();
                if (!_totesToPick.Any())
                    throw new ExceptionLocalized($"[{ReferenceNumber}] has no totes to move");

            }, Init);
            await AskToteLpn();
        }

        public async Task AskToteLpn()
        {
            var lpns = _totesToPick
                .Where(c => c.LicensePlateId != null)
                .GroupBy(c => c.LicensePlateId)
                .Select(c => c.First())
                .OrderBy(c => c.DockDoorBarcode ?? c.BinCode)
                .ThenBy(c => c.BigText)
                .ThenBy(c => c.Sscc18Code)
                .ToList();
            foreach(var lpn in lpns)
                await View.PushMessage($@"{Lang.Translate($"[{lpn.LicensePlate}] @ [{lpn.BinCode ?? lpn.DockDoorName ?? "Floor"}]")}", null, false);

            if (!lpns.Any())
            {
                var totes = _totesToPick
                    .Where(c => c.LicensePlateId == null)
                    .OrderBy(c => c.DockDoorBarcode ?? c.BinCode)
                    .ThenBy(c => c.BigText)
                    .ThenBy(c => c.Sscc18Code)
                    .ToList();

                foreach (var tote in totes)
                    await View.PushMessage($@"{Lang.Translate($"[{tote.BigText}] - [{tote.Sscc18Code}] @ [{tote.BinCode ?? tote.DockDoorName ?? "Floor"}]")}", null, false);
            }
            await View.PushMessage($@"{Lang.Translate($"To [{(DockDoorId == null ? "Floor" : DockDoor)}]")}", null, false);

            if (!lpns.Any())
            {
                _foundLpn = null;
                _foundTote = await LoopUntilGood(async () =>
                {
                    var tote = await ToteLookup(AskToteLpn);
                    if (_totesToPick.All(c => c.Id != tote.Id))
                        throw new ExceptionLocalized($"Tote [{tote.BigText}] is not in the batch");
                    return tote;
                }, AskToteLpn);
            }
            else
            {
                _foundTote = null;
                _foundLpn = await LoopUntilGood(async () =>
                {
                    var lpn = await LocationLookup(AskToteLpn, "Scan LPN...");
                    if (!lpn.IsLpn)
                        throw new ExceptionLocalized("Cannot scan bin, LPN expected");
                    if (_totesToPick.All(c => c.LicensePlateId != lpn.Id))
                        throw new ExceptionLocalized($"LPN [{lpn.LpnCode}] is not in the batch");
                    return lpn;
                }, AskToteLpn);
            }

            if (DockDoorId != null)
                await AskDockDoor();
            else
                await Process();
        }

        public async Task AskDockDoor()
        {
            _dockDoor = await LoopUntilGood(async () =>
            {
                var dock = await LocationLookup(AskDockDoor, "Scan Dock");
                if (dock.Id != DockDoorId)
                    throw new ExceptionLocalized($"Invalid Dock [{dock.LocationCode}] door, [{DockDoor}] expected");
                return dock;
            }, AskDockDoor);
            await Process();
        }

        public async Task Process()
        {
            try
            {
                var url = string.Empty;
                if(_foundLpn != null)
                    url = $"hh/floor/LicensePlateMove?{_foundLpn.QueryUrl}";
                if (_foundTote != null)
                    url += $"hh/floor/ToteMove?toteId={_foundTote.Id}";
                if (_dockDoor != null)
                    url += $"&toDockDoorId={_dockDoor.Id}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                View.InactivateMessages();
                await View.PushMessage($"{(_foundTote != null? $"Tote [{_foundTote.BigText}] - [{_foundTote.Sscc18Code}]" :"")}{(_foundLpn != null?$"LPN [{_foundLpn.LocationCode}]":"")} moved to [{(DockDoorId == null ? "Floor" : DockDoor)}]");

                if (_foundTote != null)
                    _totesToPick.Remove(_totesToPick.Single(c => c.Id == _foundTote.Id));

                if (_foundLpn != null)
                {
                    var toDelete = _totesToPick.Where(c => c.LicensePlateId == _foundLpn.Id).ToList();
                    toDelete.ForEach(c=> _totesToPick.Remove(c));
                }
                
                if (!_totesToPick.Any())
                {
                    await View.PushMessage($"[{ReferenceNumber}] has no more moves!");
                    await Init();
                }
                else
                    await AskToteLpn();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToteLpn();
            }
        }

        protected abstract Task InitChild();

        public abstract string RequireDockDoorConfigName { get; }

        public abstract string ReferenceNumber { get; }
        public abstract Guid? DockDoorId { get; }
        public abstract string DockDoor { get; }
        public abstract List<ToteLookup> Totes { get; }
    }
}