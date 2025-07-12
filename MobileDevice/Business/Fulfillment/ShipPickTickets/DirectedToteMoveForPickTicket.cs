using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipPickTickets
{
    [ViewController("main.totePickStage")]
    public class DirectedToteMoveForPickTicket : DirectedDockMoveBase
    {
        private PickTicketLookup _pickTicket;

        protected override async Task InitChild()
        { 
            _pickTicket = await LoopUntilGood(async () =>
            {
                var pickTicket = await PickTicketLookup();
                var allowedState = new[] {PickTicketState.Rating, PickTicketState.PendingDriverSignature};
                if (!allowedState.Contains(pickTicket.PickTicketState))
                    throw new ExceptionLocalized($"Truck load [{pickTicket.PickTicketNumber}] - invalid status [{pickTicket.PickTicketState}]");
                return pickTicket;
            }, InitChild);
        }

        public override string RequireDockDoorConfigName => nameof(ConfigConstants.Business_Fulfillment_Shipping_PrivateFleet_RequireDockDoor);

        public override string ReferenceNumber => _pickTicket.PickTicketNumber;
        public override Guid? DockDoorId => _pickTicket.DockDoorId;
        public override string DockDoor => _pickTicket.DockDoor;
        public override List<ToteLookup> Totes => _pickTicket.Totes;
    }
}