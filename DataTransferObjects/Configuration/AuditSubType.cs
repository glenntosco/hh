namespace Pro4Soft.DataTransferObjects.Configuration
{
    public static class AuditSubType
    {
        public static readonly string BillingCycleStart = nameof(BillingCycleStart);
        public static readonly string BillingCycleEnd = nameof(BillingCycleEnd);
        public static readonly string Pick = nameof(Pick);
        public static readonly string OneScanShip = nameof(OneScanShip);

        public static readonly string ProdOrderPick = nameof(ProdOrderPick);
        public static readonly string ProdOrderProdStep = nameof(ProdOrderProdStep);
        public static readonly string ProdOrderConsume = nameof(ProdOrderConsume);
        public static readonly string ProdOrderBuild = nameof(ProdOrderBuild);

        public static readonly string SubstituteConvert = nameof(SubstituteConvert);
        
        public static readonly string ReceivePo = nameof(ReceivePo);
        public static readonly string UnreceivePo = nameof(UnreceivePo);
        public static readonly string NonPoReceive = nameof(NonPoReceive);
        public static readonly string ReceiveRma = nameof(ReceiveRma);
        public static readonly string UnreceiveRma = nameof(UnreceiveRma);
        public static readonly string NonRmaReceive = nameof(NonRmaReceive);
        public static readonly string SmallParcelShip = nameof(SmallParcelShip);
        public static readonly string ExternalShip = nameof(ExternalShip);
        public static readonly string TruckLoadShip = nameof(TruckLoadShip);
        public static readonly string PrivateFleetShip = nameof(PrivateFleetShip);

        public static readonly string MasterTruckLoadShip = nameof(MasterTruckLoadShip);

        public static readonly string VendorReturn = nameof(VendorReturn);
        public static readonly string CustomerReturn = nameof(CustomerReturn);
        public static readonly string AdjustOut = nameof(AdjustOut);

        public static readonly string AdjustIn = nameof(AdjustIn);
        public static readonly string PacksizeConvert = nameof(PacksizeConvert);
        public static readonly string PacksizeBreakdown = nameof(PacksizeBreakdown);
        public static readonly string Letdown = nameof(Letdown);
        public static readonly string LicensePlateMove = nameof(LicensePlateMove);
        public static readonly string ProductMove = nameof(ProductMove);
        public static readonly string WarehouseMove = nameof(WarehouseMove);
        public static readonly string BinMove = nameof(BinMove);
        public static readonly string UnpickPickTicket = nameof(UnpickPickTicket);
        public static readonly string UnpickTote = nameof(UnpickTote);
        public static readonly string ToteFloorMove = nameof(ToteFloorMove);
        public static readonly string ToteDockDoorMove = nameof(ToteDockDoorMove);
        public static readonly string ToteBinMove = nameof(ToteBinMove);
        public static readonly string ToteLpnMove = nameof(ToteLpnMove);
        public static readonly string ShipToteCount = nameof(ShipToteCount);
        public static readonly string DeliveryToteCount = nameof(DeliveryToteCount);
        public static readonly string Repackage = nameof(Repackage);
        public static readonly string Init = nameof(Init);
        public static readonly string CycleCount = nameof(CycleCount);
        public static readonly string CycleCountApproval = nameof(CycleCountApproval);

        public static readonly string PalletPrinting = nameof(PalletPrinting);

        public static readonly string LoginWeb = nameof(LoginWeb);
        public static readonly string LoginHandheld = nameof(LoginHandheld);
        public static readonly string LoginExternal = nameof(LoginExternal);
        public static readonly string LoginImpersonate = nameof(LoginImpersonate);

        public static readonly string LogoutRegular = nameof(LogoutRegular);
        public static readonly string LogoutNoLicense = nameof(LogoutNoLicense);
        public static readonly string LogoutUserDuplicate = nameof(LogoutUserDuplicate);
        public static readonly string LogoutUserDeactivated = nameof(LogoutUserDeactivated);
        public static readonly string LogoutByAdmin = nameof(LogoutByAdmin);
        public static readonly string LogoutExpiry = nameof(LogoutExpiry);
    }
}