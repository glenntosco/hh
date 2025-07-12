using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipTruckLoads
{
    [ViewController("main.totePickStageTruckLoad")]
    public class DirectedToteMoveForTruckLoad : DirectedDockMoveBase
    {
        private TruckLoadLookup _truckLoad;

        protected override async Task InitChild()
        {
            _truckLoad = await LoopUntilGood(async () =>
            {
                var truckLoad = await TruckLoadLookup(null, true);
                if(truckLoad.MasterTruckLoadId != null)
                    truckLoad = await TruckLoadLookup(truckLoad.MasterTruckLoadId, true);
                var allowedStates = new[] {TruckLoadState.Staging, TruckLoadState.PendingShipperSignature, TruckLoadState.PendingCarrierSignature};
                if (!allowedStates.Contains(truckLoad.TruckLoadState))
                    throw new ExceptionLocalized($"Truck load [{truckLoad.TruckLoadNumber}] - invalid status [{truckLoad.TruckLoadState}]");
                return truckLoad;
            }, InitChild);
        }

        public override string RequireDockDoorConfigName => nameof(ConfigConstants.Business_Fulfillment_Shipping_TruckLoad_Operations_RequireDockDoor);

        public override string ReferenceNumber => _truckLoad.TruckLoadNumber;
        public override Guid? DockDoorId => _truckLoad.DockDoorId;
        public override string DockDoor => _truckLoad.DockDoor;
        public override List<ToteLookup> Totes => _truckLoad.Totes;
    }
}