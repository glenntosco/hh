using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class UserTaskBase
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }

        public string Instructions { get; set; }
        public UserTaskType TaskTypeEnum { get; set; } = UserTaskType.ProductMove;
    }

    public enum UserTaskType
    {
        PurchaseOrder,
        PickTicket,
        CustomerReturn,
        TruckLoad,
        MasterTruckLoad,
        ProductionOrder,
        ProductMove,
        BinMove,
        LpnMove,
        CycleCount
    }

    public class ProductMoveTask: UserTaskBase
    {
        public Guid ProductId { get; set; }
        
        public Guid FromBinId { get; set; }
        public string FromLpn { get; set; }

        public Guid ToBinId { get; set; }
        public string ToLpn { get; set; }

        public string Expiry { get; set; }
        public Guid PacksizeId { get; set; }
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }
        public decimal Quantity { get; set; }
    }
}
