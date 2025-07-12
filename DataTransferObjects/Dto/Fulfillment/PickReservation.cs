using System;
using Pro4Soft.DataTransferObjects.Dto.Floor;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class PickReservation
    {
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? DetailId { get; set; }
        public Guid? PickTicketId { get; set; }
        public Guid? ProductionOrderId { get; set; }
        public int PickSequence { get; set; }
        public Guid? BinId { get; set; }
        public Guid? ZoneId { get; set; }
        public string BinSequenceNumber { get; set; }
        public string BinCode { get; set; }
        public string Sscc18Code { get; set; }
        public string BigText { get; set; }
        public bool FullPalletPick { get; set; }
        public Guid? LpnId { get; set; }
        public string Lpn { get; set; }
        public ProductDetails Product { get; set; }
        public decimal QuantityToPick { get; set; }

        public Guid? PacksizeId { get; set; }
        public int? Packsize { get; set; }
        public string LotNumber { get; set; }
        public string ExpiryString { get; set; }
        public DateTime? Expiry { get; set; }
        
        public string GetPickBin()
        {
            return !string.IsNullOrWhiteSpace(Lpn) ? $"[{Lpn}] @ [{BinCode}]" : $"[{BinCode}]";
        }
    }
}