// ReSharper disable InconsistentNaming

using System;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;

namespace Pro4Soft.DataTransferObjects.Configuration
{
    public static class 
        ConfigConstants
	{
        #region Setup

        #region System
        [ConfigDefinition(ConfigType.Int, "Number of minutes before session expires", 120)]
		public const string Setup_System_SessionExpiry = nameof(Setup_System_SessionExpiry);

        [ConfigDefinition(ConfigType.Int, "Maximum number of records an audit report should return", 1000)]
        public const string Setup_System_AuditRecordsLimit = nameof(Setup_System_AuditRecordsLimit);

        [ConfigDefinition(ConfigType.Int, "Maximum number of records in history report", 100)]
        public const string Setup_System_HistoryRecordsLimit = nameof(Setup_System_HistoryRecordsLimit);

        [ConfigDefinition(ConfigType.Int, "Maximum number of records a SQL query should return", 10000)]
        public const string Setup_System_QueryRecordsLimit = nameof(Setup_System_QueryRecordsLimit);

        [ConfigDefinition(ConfigType.Int, "Maximum number of records to be allowed for Grid export", 50000)]
        public const string Setup_System_GridExportRecordLimit = nameof(Setup_System_GridExportRecordLimit);
        #endregion

        #region UserInterface
        [ConfigDefinition(ConfigType.Bool, "Popup QR code on mouse over", false, null, false)]
        public const string Setup_UserInterface_PopupQrCodes = nameof(Setup_UserInterface_PopupQrCodes);

        [ConfigDefinition(ConfigType.Bool, "Display product images", true)]
        public const string Setup_UserInterface_DisplayProductImages = nameof(Setup_UserInterface_DisplayProductImages);
        #endregion

        #region Setup_PasswordPolicy
        [ConfigDefinition(ConfigType.Bool, "Whether password policy should be enforced for administrators", false)]
        public const string Setup_PasswordPolicy_EnforceAdministrators = nameof(Setup_PasswordPolicy_EnforceAdministrators);

        [ConfigDefinition(ConfigType.Bool, "Whether direct passwords change by administrators", true)]
        public const string Setup_PasswordPolicy_AllowAdminPasswordChange = nameof(Setup_PasswordPolicy_AllowAdminPasswordChange);

        [ConfigDefinition(ConfigType.Int, "Minimum password length", 8)]
        public const string Setup_PasswordPolicy_MinimumPasswordLength = nameof(Setup_PasswordPolicy_MinimumPasswordLength);

        [ConfigDefinition(ConfigType.Int, "Maximum password length", 16)]
        public const string Setup_PasswordPolicy_MaximumPasswordLength = nameof(Setup_PasswordPolicy_MaximumPasswordLength);

        [ConfigDefinition(ConfigType.Bool, "Whether password must contain upper and lower case characters", true)]
        public const string Setup_PasswordPolicy_UpperCaseRequired = nameof(Setup_PasswordPolicy_UpperCaseRequired);

        [ConfigDefinition(ConfigType.Bool, "Whether password must contain numbers", true)]
        public const string Setup_PasswordPolicy_NumbersRequired = nameof(Setup_PasswordPolicy_NumbersRequired);

        [ConfigDefinition(ConfigType.Bool, "Whether password must contain special characters", true)]
        public const string Setup_PasswordPolicy_SpecialCharactersRequired = nameof(Setup_PasswordPolicy_SpecialCharactersRequired);

        [ConfigDefinition(ConfigType.Int, "If a user doesn't change it's password within that period an account will be deactivated", 60)]
        public const string Setup_PasswordPolicy_DaysExpiry = nameof(Setup_PasswordPolicy_DaysExpiry);

        [ConfigDefinition(ConfigType.Int, "Number of days after which a warning should be displayed on password change", 14)]
        public const string Setup_PasswordPolicy_PasswordWarningThreshold = nameof(Setup_PasswordPolicy_PasswordWarningThreshold);

        [ConfigDefinition(ConfigType.Int, "Prevent allowing last N passwords to repeat", 5)]
        public const string Setup_PasswordPolicy_PasswordNonRepeatCount = nameof(Setup_PasswordPolicy_PasswordNonRepeatCount);
        #endregion

        #region Integration
        [ConfigDefinition(ConfigType.Bool, "Controls whether Data Entry should be enabled or data is expected to be created/updated via Integration only", true)]
        public const string Setup_Integration_EnableDataEntry = nameof(Setup_Integration_EnableDataEntry);

        [ConfigDefinition(ConfigType.Bool, "Controls whether Upload failure reprocessing should be enabled", false)]
        public const string Setup_Integration_UploadControls = nameof(Setup_Integration_UploadControls);

        [ConfigDefinition(ConfigType.Int, "Number of hours after which a failed upload should be attempted again", 24)]
        public const string Setup_Integration_ReUploadGracePeriod = nameof(Setup_Integration_ReUploadGracePeriod);

        [ConfigDefinition(ConfigType.Bool, "Whether picktickets should be attempted to re-upload", false)]
        public const string Setup_Integration_ReUploadPicktickets = nameof(Setup_Integration_ReUploadPicktickets);
        
        [ConfigDefinition(ConfigType.Int, "Number of times a pick ticket should be attempted to re-upload before permanent stop", 5)]
        public const string Setup_Integration_PickticketUploadCount = nameof(Setup_Integration_PickticketUploadCount);

        [ConfigDefinition(ConfigType.Bool, "Whether pick tickets should be attempted to re-upload", false)]
        public const string Setup_Integration_ReUploadPurchaseOrders = nameof(Setup_Integration_ReUploadPurchaseOrders);

        [ConfigDefinition(ConfigType.Int, "Number of times a PO should be attempted to re-upload before permanent stop", 5)]
        public const string Setup_Integration_PurchaseOrderUploadCount = nameof(Setup_Integration_PurchaseOrderUploadCount);

        [ConfigDefinition(ConfigType.Bool, "Whether production orders should be attempted to re-upload", false)]
        public const string Setup_Integration_ReUploadProductionOrders = nameof(Setup_Integration_ReUploadProductionOrders);

        [ConfigDefinition(ConfigType.Int, "Number of times a production order should be attempted to re-upload before permanent stop", 5)]
        public const string Setup_Integration_ProductionOrderUploadCount = nameof(Setup_Integration_ProductionOrderUploadCount);

        [ConfigDefinition(ConfigType.Bool, "Whether customer returns should be attempted to re-upload", false)]
        public const string Setup_Integration_ReUploadReturns = nameof(Setup_Integration_ReUploadReturns);

        [ConfigDefinition(ConfigType.Int, "Number of times a customer return should be attempted to re-upload before permanent stop", 5)]
        public const string Setup_Integration_ReturnsUploadCount = nameof(Setup_Integration_ReturnsUploadCount);

        [ConfigDefinition(ConfigType.Bool, "Whether every entity should be automatically uploaded on closure. This configuration should be disabled if tenant is running integrations, otherwise set to true to not interfece with Four Wall report", false)]
        public const string Setup_Integration_IntegrationAutoUpload = nameof(Setup_Integration_IntegrationAutoUpload);
        #endregion

        #region Communication
        #region Email
        [ConfigDefinition(ConfigType.String, "Smtp server for email sending")]
        public const string Setup_Communication_Email_SmtpHost = nameof(Setup_Communication_Email_SmtpHost);

        [ConfigDefinition(ConfigType.Int, "Smtp server port for email sending")]
        public const string Setup_Communication_Email_SmtpPort = nameof(Setup_Communication_Email_SmtpPort);

        [ConfigDefinition(ConfigType.Bool, "Whether to use SSL channel", false)]
        public const string Setup_Communication_Email_UseSSL = nameof(Setup_Communication_Email_UseSSL);

        [ConfigDefinition(ConfigType.String, "Domain name for credential verification")]
        public const string Setup_Communication_Email_Domain = nameof(Setup_Communication_Email_Domain);

        [ConfigDefinition(ConfigType.String, "Username associated with sending email")]
        public const string Setup_Communication_Email_Username = nameof(Setup_Communication_Email_Username);

        [ConfigDefinition(ConfigType.Password, "Password associated with credentials")]
        public const string Setup_Communication_Email_Password = nameof(Setup_Communication_Email_Password);

        [ConfigDefinition(ConfigType.String, "From address for automated emails")]
        public const string Setup_Communication_Email_FromEmail = nameof(Setup_Communication_Email_FromEmail);

        [ConfigDefinition(ConfigType.String, "Catch all recipients for automated emails. All emails sent via system will be included. Comma separated")]
        public const string Setup_Communication_Email_To = nameof(Setup_Communication_Email_To);

        [ConfigDefinition(ConfigType.String, "Catch all (CC) recipients for automated emails. All emails sent via system will be included. Comma separated")]
        public const string Setup_Communication_Email_Cc = nameof(Setup_Communication_Email_Cc);

        [ConfigDefinition(ConfigType.String, "Catch all (BCC) recipients for automated emails. All emails sent via system will be included. Comma separated")]
        public const string Setup_Communication_Email_Bcc = nameof(Setup_Communication_Email_Bcc);
        #endregion

        #endregion

        #endregion

        //Business
        #region Receiving

        #region Data Entry
        [ConfigDefinition(ConfigType.String, "PO number prefix", "PO-")]
        public const string Business_Receiving_DataEntry_PoNumberPrefix = nameof(Business_Receiving_DataEntry_PoNumberPrefix);

        [ConfigDefinition(ConfigType.Bool, "Whether a custom PO number could be entered during data entry", true)]
        public const string Business_Receiving_DataEntry_AllowCustomPoNumber = nameof(Business_Receiving_DataEntry_AllowCustomPoNumber);

        [ConfigDefinition(ConfigType.MultiSelect, "Duplicate SKUs line behaviour. Allow - allows duplicates, Warning - Will present a prompt warning of a duplicate. Prevent - will error on attempt to create a duplicate.", nameof(DuplicateLineSkuBehaviour.Allow),
            new[] { nameof(DuplicateLineSkuBehaviour.Allow), nameof(DuplicateLineSkuBehaviour.Prompt), nameof(DuplicateLineSkuBehaviour.Prevent) })]
        public const string Business_Receiving_DataEntry_DuplicateSkuBehaviour = nameof(Business_Receiving_DataEntry_DuplicateSkuBehaviour);

        [ConfigDefinition(ConfigType.MultiSelect, "Data entry unit of measure for packsize controlled items", "Packs", new[] { "Packs", "Eaches" })]
        public const string Business_Receiving_DataEntry_PacksizeEntryType = nameof(Business_Receiving_DataEntry_PacksizeEntryType);

        [ConfigDefinition(ConfigType.String, "Comma separated list of fields that are available for Apply Field(s) function in Purchase order list", "")]
        public const string Business_Receiving_DataEntry_BulkUpdateFields = nameof(Business_Receiving_DataEntry_BulkUpdateFields);
        #endregion

        #region Operations
        [ConfigDefinition(ConfigType.Bool, "Whether a PO should be allowed to receive without assigned Operator", true)]
        public const string Business_Receiving_Operations_AllowPoReceivingWithoutAssignment = nameof(Business_Receiving_Operations_AllowPoReceivingWithoutAssignment);

        [ConfigDefinition(ConfigType.Bool, "Whether a PO should be allowed to receive without Dock door assigned", true)]
        public const string Business_Receiving_Operations_AllowPoReceivingWithoutDockDoor = nameof(Business_Receiving_Operations_AllowPoReceivingWithoutDockDoor);

        [ConfigDefinition(ConfigType.Int, "Whether to allow expired product and expiry allowance in days. Ex: 5 mean admitting product will expire in 5 days, any value less than that will be rejected. Ex: -5 admitting product has expired for 5 days, any value that has been expired for more than 5 days will be rejected", 0)]
        public const string Business_Receiving_Operations_ExpiryAllowance = nameof(Business_Receiving_Operations_ExpiryAllowance);

        [ConfigDefinition(ConfigType.Int, "Over-Receive threshold. How much more of the product could be received (in %) compared to ordered quantity. Zero means - no over receiving will be accepted", 50)]
        public const string Business_Receiving_Operations_OverReceiveThreshold = nameof(Business_Receiving_Operations_OverReceiveThreshold);

        [ConfigDefinition(ConfigType.Bool, "Whether to include photos of order on receiving slip report", false)]
        public const string Business_Receiving_Operations_IncludePhotosOnRecvSlip = nameof(Business_Receiving_Operations_IncludePhotosOnRecvSlip);
        #endregion

        #region Handheld
        [ConfigDefinition(ConfigType.Bool, "Whether to show expected quantity during receiving", true)]
        public const string Business_Receiving_Handheld_ShowExpectedQuantity = nameof(Business_Receiving_Handheld_ShowExpectedQuantity);

        [ConfigDefinition(ConfigType.Bool, "Whether a Handheld operator should be allowed to close PO", true)]
        public const string Business_Receiving_Handheld_AllowHandheldClose = nameof(Business_Receiving_Handheld_AllowHandheldClose);

        [ConfigDefinition(ConfigType.Bool, "Whether a Handheld operator should collect missing Weight and Dimensions of a product", false)]
        public const string Business_Receiving_Handheld_CollectWeightAndDimensions = nameof(Business_Receiving_Handheld_CollectWeightAndDimensions);

        [ConfigDefinition(ConfigType.Bool, "Whether a Handheld operator should collect missing Barcode for Product and/or Packsizes", false)]
        public const string Business_Receiving_Handheld_CollectBarcode = nameof(Business_Receiving_Handheld_CollectBarcode);

        [ConfigDefinition(ConfigType.Bool, "Print labels during receiving", false)]
        public const string Business_Receiving_Handheld_PrintLabels = nameof(Business_Receiving_Handheld_PrintLabels);

        [ConfigDefinition(ConfigType.Bool, "Whether the system should suggest a bin to receive to", false)]
        public const string Business_Receiving_Handheld_SuggestPutawayBin = nameof(Business_Receiving_Handheld_SuggestPutawayBin);
        #endregion

        #region Automation
        [ConfigDefinition(ConfigType.Bool, "Whether to close POs automatically upon receipt", true)]
        public const string Business_Receiving_Automation_AutoCloseReceivedPos = nameof(Business_Receiving_Automation_AutoCloseReceivedPos);

        [ConfigDefinition(ConfigType.Bool, "Whether to create a backorder on closing with outstanding quantities", false)]
        public const string Business_Receiving_Automation_GenerateBackOrderOnClose = nameof(Business_Receiving_Automation_GenerateBackOrderOnClose);

        [ConfigDefinition(ConfigType.Bool, "Whether to auto release backorders", false)]
        public const string Business_Receiving_Automation_AutoReleaseBackOrder = nameof(Business_Receiving_Automation_AutoReleaseBackOrder);
        #endregion

        #region DocumentPrinting
        [ConfigDefinition(ConfigType.MultilineString, "Default message that will be printed on Receiving Slip document (PDF) in the Footer section. Typically contains Disclaimer information. Could be overriden on Client or Partner level")]
        public const string Business_Receiving_DocumentPrinting_ReceivingSlipDisclaimer = nameof(Business_Receiving_DocumentPrinting_ReceivingSlipDisclaimer);
        #endregion

        #region EmailNotification
        [ConfigDefinition(ConfigType.Bool, "Whether to send out emails on Closure of Purchase order", false)]
        public const string Business_Receiving_EmailNotification_SendEmail = nameof(Business_Receiving_EmailNotification_SendEmail);

        [ConfigDefinition(ConfigType.String, "Comma separated list of recipients. Available keywords: (@VendorEmail, @ClientEmail)")]
        public const string Business_Receiving_EmailNotification_To = nameof(Business_Receiving_EmailNotification_To);

        [ConfigDefinition(ConfigType.String, "Comma separated list of CC (Carbon Copy). Available keywords: (@VendorEmail, @ClientEmail)")]
        public const string Business_Receiving_EmailNotification_Cc = nameof(Business_Receiving_EmailNotification_Cc);

        [ConfigDefinition(ConfigType.String, "Comma separated list of BCC (Blind Carbon Copy). Available keywords: (@VendorEmail, @ClientEmail)")]
        public const string Business_Receiving_EmailNotification_Bcc = nameof(Business_Receiving_EmailNotification_Bcc);

        [ConfigDefinition(ConfigType.String, "Email Subject. Available keywords: (@PurchaseOrderNumber, @ReceiveDate, @VendorName, @ClientName)")]
        public const string Business_Receiving_EmailNotification_Subject = nameof(Business_Receiving_EmailNotification_Subject);

        [ConfigDefinition(ConfigType.EmailBody, "Email Body. Available keywords: (@PurchaseOrderNumber, @ReceiveDate, @VendorName, @ClientName)")]
        public const string Business_Receiving_EmailNotification_Body = nameof(Business_Receiving_EmailNotification_Body);
        #endregion

        #endregion

        #region Production

        #region Operations
        [ConfigDefinition(ConfigType.String, "Production order number prefix", "PRD-")]
        public const string Business_Production_Operations_NumberPrefix = nameof(Business_Production_Operations_NumberPrefix);

        [ConfigDefinition(ConfigType.Bool, "Whether a custom product order number could be entered during data entry", true)]
        public const string Business_Production_Operations_AllowCustomProdNumber = nameof(Business_Production_Operations_AllowCustomProdNumber);

        [ConfigDefinition(ConfigType.Bool, "Whether a production order should be allowed to building more than was Allocated.", false)]
        public const string Business_Production_Operations_AllowOverProduction = nameof(Business_Production_Operations_AllowOverProduction);

        [ConfigDefinition(ConfigType.Bool, "Whether a production order can be completed if it's missing components in production area.", false)]
        public const string Business_Production_Operations_AllowCompletingWithMissingComponents = nameof(Business_Production_Operations_AllowCompletingWithMissingComponents);

        [ConfigDefinition(ConfigType.MultiSelect, "Production consumption mode. 'Ordered quantity' will consume according to ordered quantities regardless of quantities produced. 'Production Proportionate' will consume proportionally to produced quantity", nameof(ProductionConsumptionMode.OrderedQuantity), new[]
        {
            nameof(ProductionConsumptionMode.OrderedQuantity), nameof(ProductionConsumptionMode.ProductionProportionate),
        })]
        public const string Business_Production_Operations_ConsumptionMode = nameof(Business_Production_Operations_ConsumptionMode);


        #endregion

        #region Picking
        [ConfigDefinition(ConfigType.Bool, "If enabled, will allow non administrator users to short picks", true)]
        public const string Business_Production_Picking_AllowShorting = nameof(Business_Production_Picking_AllowShorting);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will allow users to skip picks", false)]
        public const string Business_Production_Picking_AllowSkipping = nameof(Business_Production_Picking_AllowSkipping);

        [ConfigDefinition(ConfigType.Bool, "Mark for Cycle count on Shorting", false)]
        public const string Business_Production_Picking_MarkForCycleCountOnShort = nameof(Business_Production_Picking_MarkForCycleCountOnShort);
        #endregion

        #region Allocation
        [ConfigDefinition(ConfigType.String, "Comma separated list of zones to include in allocation. Ex: 'A1,B2,C3'")]
        public const string Business_Production_Allocation_ZonesToInclude = nameof(Business_Production_Allocation_ZonesToInclude);

        [ConfigDefinition(ConfigType.String, "Comma separated list of zones to include for work area. Ex: 'A1,B2,C3'")]
        public const string Business_Production_Allocation_WorkAreaZones = nameof(Business_Production_Allocation_WorkAreaZones);

        [ConfigDefinition(ConfigType.Int, "Minimum number of days to be allowed on an expiry product to be allocated", 145)]
        public const string Business_Production_Allocation_ExpiryAllowance = nameof(Business_Production_Allocation_ExpiryAllowance);

        [ConfigDefinition(ConfigType.Bool, "Whether to automatically assign work area. First empty bin will be used in work area zones will be assigned", false)]
        public const string Business_Production_Allocation_AutoAssignWorkArea = nameof(Business_Production_Allocation_AutoAssignWorkArea);

        [ConfigDefinition(ConfigType.MultiSelect, "Options for allocation behaviour. FIFO - First in, first out. LIFO - Last in, first out. BinName - Based on alphanumeric bin sorting. MostProductFirst - Attempt to consume product from bin/lpn with most quantity first. LeastProductFirst - Attempt to consume product from bin/lpn with least quantity first", nameof(AllocationStyleEnum.BinName),
            new[] { nameof(AllocationStyleEnum.FIFO), nameof(AllocationStyleEnum.LIFO), nameof(AllocationStyleEnum.BinName), nameof(AllocationStyleEnum.MostProductFirst), nameof(AllocationStyleEnum.LeastProductFirst) })]
        public const string Business_Production_Allocation_AllocationStyle = nameof(Business_Production_Allocation_AllocationStyle);

        [ConfigDefinition(ConfigType.MultiSelect, "Options for short behaviour. PickShort - Pick and ship what is available. HoldShort - Reserve what's available and hold", nameof(ShortOptionsEnum.PickShort),
            new[] { nameof(ShortOptionsEnum.PickShort), nameof(ShortOptionsEnum.HoldShort) })]
        public const string Business_Production_Allocation_ShortOptions = nameof(Business_Production_Allocation_ShortOptions);

        [ConfigDefinition(ConfigType.MultiSelect, "Expiry controlled items override Allocation style", nameof(ExpiryAllocationPrecedenceEnum.OldestFirst),
            new[] { nameof(ExpiryAllocationPrecedenceEnum.NoPrecedence), nameof(ExpiryAllocationPrecedenceEnum.OldestFirst), nameof(ExpiryAllocationPrecedenceEnum.NewestFirst) })]
        public const string Business_Production_Allocation_ExpiryAllocationPrecedence = nameof(Business_Production_Allocation_ExpiryAllocationPrecedence);

        [ConfigDefinition(ConfigType.MultiSelect, "Lot controlled items allocation style", nameof(LotAllocationStyleEnum.Inherit),
            new[] { nameof(LotAllocationStyleEnum.Inherit), nameof(LotAllocationStyleEnum.Alphabetical), nameof(LotAllocationStyleEnum.MaxQtyFirst), nameof(LotAllocationStyleEnum.MinQtyFirst) })]
        public const string Business_Production_Allocation_LotAllocationStyle = nameof(Business_Production_Allocation_LotAllocationStyle);

        [ConfigDefinition(ConfigType.Bool, "Allow partial lot allocation. If enabled, the system will allow multiple lots to be picked", true)]
        public const string Business_Production_Allocation_AllowPartialLots = nameof(Business_Production_Allocation_AllowPartialLots);

        [ConfigDefinition(ConfigType.Bool, "Whether zone sequence setup on Allocation screen should be enforced during allocation (ex: Zones A, B, C will enforce stock to be first allocated from zone A then B then C)", false)]
        public const string Business_Production_Allocation_ObeyZoneSequence = nameof(Business_Production_Allocation_ObeyZoneSequence);

        [ConfigDefinition(ConfigType.Bool, "Include LPNs in Bulk Zones during allocation to pick/replenish from", true)]
        public const string Business_Production_Allocation_AllocateBulk = nameof(Business_Production_Allocation_AllocateBulk);

        [ConfigDefinition(ConfigType.Bool, "Whether to hold order for Letdown", true)]
        public const string Business_Production_Allocation_HoldLetdown = nameof(Business_Production_Allocation_HoldLetdown);

        [ConfigDefinition(ConfigType.Bool, "Whether to issue packsize breakdown requests and hold order", true)]
        public const string Business_Production_Allocation_PacksizeBreakdown = nameof(Business_Production_Allocation_PacksizeBreakdown);

        [ConfigDefinition(ConfigType.Bool, "Whether to allow allocating eaches for packsize controlled items", true)]
        public const string Business_Production_Allocation_AllocateEaches = nameof(Business_Production_Allocation_AllocateEaches);

        //[ConfigDefinition(ConfigType.Bool, "Whether auto allocation should be decoupled from handheld functions. Improved performance of Packsize breakdown and LPN Letdown", false)]
        //public const string Business_Production_Allocation_DecoupledAllocation = nameof(Business_Production_Allocation_DecoupledAllocation);
        #endregion

        #region Handheld
        [ConfigDefinition(ConfigType.Bool, "Print labels during production", false)]
        public const string Business_Production_Handheld_PrintLabels = nameof(Business_Production_Handheld_PrintLabels);

        [ConfigDefinition(ConfigType.Bool, "Whether a Handheld operator should collect missing Weight and Dimensions of a product", false)]
        public const string Business_Production_Handheld_CollectWeightAndDimensions = nameof(Business_Production_Handheld_CollectWeightAndDimensions);

        [ConfigDefinition(ConfigType.Bool, "Whether a Handheld operator should collect missing Barcode for Product and/or Packsizes", false)]
        public const string Business_Production_Handheld_CollectBarcode = nameof(Business_Production_Handheld_CollectBarcode);
        #endregion

        #region Automation
        [ConfigDefinition(ConfigType.Bool, "Whether to reset user assignment throught the lifecycle of a sroduction order", false)]
        public const string Business_Production_Automation_ResetUserOnChange = nameof(Business_Production_Automation_ResetUserOnChange);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto allocate upon being released to floor. Default allocation settings will be used", false)]
        public const string Business_Production_Automation_AutoAllocateOnReleaseToFloor = nameof(Business_Production_Automation_AutoAllocateOnReleaseToFloor);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto print replenishment/picking ticket on allocation. Default waving settings will be used", false)]
        public const string Business_Production_Automation_AutoPrintOnAllocation = nameof(Business_Production_Automation_AutoPrintOnAllocation);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto Complete and backflus work area inventory on last build", false)]
        public const string Business_Production_Automation_AutoCompleteOnLastBuild = nameof(Business_Production_Automation_AutoCompleteOnLastBuild);
        
        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto Close on Completion", false)]
        public const string Business_Production_Automation_AutoCloseOnComplete = nameof(Business_Production_Automation_AutoCloseOnComplete);
        #endregion

        #endregion

        #region Fulfillment

        #region Shipping
        
        #region TruckLoad
        #region Operations
        [ConfigDefinition(ConfigType.String, "Truck load number prefix", "LD-")]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_TruckLoadNumberPrefix = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_TruckLoadNumberPrefix);

        [ConfigDefinition(ConfigType.String, "GS1 BOL Company Prefix [8 digits]. Used in Bill Of Lading number generation unless one setup on a 3PL client", "11111111")]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_BolPrefix = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_BolPrefix);

        [ConfigDefinition(ConfigType.MultiSelect, "Default value for 'Charge Terms' option", nameof(FreightChargeTerms.Empty), new[]
        {
            nameof(FreightChargeTerms.Empty), nameof(FreightChargeTerms.Collect), nameof(FreightChargeTerms.Prepaid), nameof(FreightChargeTerms.ThirdParty)
        })]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_ChargeTerms = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_ChargeTerms);

        [ConfigDefinition(ConfigType.MultiSelect, "Default value for 'Freight Counted' option", nameof(FreightCountedType.Empty), new[]
        {
            nameof(FreightCountedType.Empty), nameof(FreightCountedType.ByShipper), nameof(FreightCountedType.ByDriverPallets), nameof(FreightCountedType.ByDriverPieces)
        })]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_FreightCounted = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_FreightCounted);

        [ConfigDefinition(ConfigType.MultiSelect, "Default value for 'Trailer Loaded' option", nameof(TrailerLoadedType.Empty), new[]
        {
            nameof(TrailerLoadedType.Empty), nameof(TrailerLoadedType.ByShipper),nameof(TrailerLoadedType.ByDriver)
        })]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_TrailerLoaded = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_TrailerLoaded);

        [ConfigDefinition(ConfigType.Bool, "Whether a BOL document will be allowed to print without signatures. Applies only to regular users, administrators will be able to print regardless of this setting", false)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_AllowPrintingBolWithoutSignature = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_AllowPrintingBolWithoutSignature);

        [ConfigDefinition(ConfigType.Bool, "Whether a BOL document should be automatically printed upon shipping", false)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_AutoPrintOnShip = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_AutoPrintOnShip);

        [ConfigDefinition(ConfigType.Bool, "Default value for 'Shipper e-sign' option", true)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_ShipperESign = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_ShipperESign);

        [ConfigDefinition(ConfigType.Bool, "Default value for 'Carrier e-sign' option", true)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_CarrierESign = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_CarrierESign);

        [ConfigDefinition(ConfigType.MultiSelect, "Truck load consolidation restrictions, less relaxed matching rules are used for loads bound for distribution centers. 'Match Full Address' - Totes full address must match with truckload. 'Match Route' - Totes route must match. 'Match Zip/Postal' - Zip/postal must match. 'Match City' - City must match. 'Match State/Province' - State/Province must match. 'Any' - all totes for this customer will be available for consolidation regardless of shipping information.",
            nameof(TruckLoadConsolidationType.MatchFullAddress), new[]
        {
            nameof(TruckLoadConsolidationType.MatchFullAddress),
            nameof(TruckLoadConsolidationType.MatchRoute),
            nameof(TruckLoadConsolidationType.MatchApptNumber),
            nameof(TruckLoadConsolidationType.MatchZipPostal),
            nameof(TruckLoadConsolidationType.MatchCity),
            nameof(TruckLoadConsolidationType.MatchStateProvince),
            nameof(TruckLoadConsolidationType.Any),
        })]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_ConsolidationRules = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_ConsolidationRules);

        [ConfigDefinition(ConfigType.MultiSelect, "Truck load loading mode. Either pallet or totes",
            nameof(ToteType.Pallet), new[]
            {
                nameof(ToteType.Carton),
                nameof(ToteType.Pallet),
            })]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_LoadingMode = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_LoadingMode);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Truck Loads to be assigned to dock doors for shipping", false)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_RequireDockDoor = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_RequireDockDoor);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Totes to be moved to Dock doors and require to match Dock door to Truck load dock door assignment", false)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_RequireDockDoorMatching = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_RequireDockDoorMatching);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Truck Loads to have Freight Charge terms setup before Staging", false)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_RequireFreightChargeTerms = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_RequireFreightChargeTerms);

        [ConfigDefinition(ConfigType.Double, "Default pallet weight. If filled in, will automatically calculate total pallet weight on Staging", 0.0)]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_PalletWeight = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_PalletWeight);

        [ConfigDefinition(ConfigType.MultilineString, "Message to be printed on BOL for Hazmat products. Should contain contact information of a Hazmat removal service")]
        public const string Business_Fulfillment_Shipping_TruckLoad_Operations_HazmatMessage = nameof(Business_Fulfillment_Shipping_TruckLoad_Operations_HazmatMessage);
        #endregion
        
        #region EmailNotification
        [ConfigDefinition(ConfigType.Bool, "Whether to send out emails on Closure of Truck loads", false)]
        public const string Business_Fulfillment_Shipping_TruckLoad_EmailNotification_SendEmail = nameof(Business_Fulfillment_Shipping_TruckLoad_EmailNotification_SendEmail);

        [ConfigDefinition(ConfigType.String, "Comma separated list of recipients. Available keywords: (@CustomerEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_TruckLoad_EmailNotification_To = nameof(Business_Fulfillment_Shipping_TruckLoad_EmailNotification_To);

        [ConfigDefinition(ConfigType.String, "Comma separated list of CC (Carbon Copy). Available keywords: (@CustomerEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Cc = nameof(Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Cc);

        [ConfigDefinition(ConfigType.String, "Comma separated list of BCC (Blind Carbon Copy). Available keywords: (@CustomerEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Bcc = nameof(Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Bcc);

        [ConfigDefinition(ConfigType.String, "Email Subject. Available keywords: (@TruckLoadNumber, @BillOfLadingNumber, @ShipDate, @CustomerName, @ClientName)")]
        public const string Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Subject = nameof(Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Subject);

        [ConfigDefinition(ConfigType.EmailBody, "Email Body. Available keywords: (@TruckLoadNumber, @BillOfLadingNumber, @ShipDate, @CustomerName, @ClientName)")]
        public const string Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Body = nameof(Business_Fulfillment_Shipping_TruckLoad_EmailNotification_Body);
        #endregion
        
        #endregion

        #region MasterTruckLoad
        #region Operations
        [ConfigDefinition(ConfigType.String, "Master Truck load number prefix", "MLD-")]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_TruckLoadNumberPrefix = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_TruckLoadNumberPrefix);

        [ConfigDefinition(ConfigType.MultiSelect, "Default value for 'Charge Terms' option", nameof(FreightChargeTerms.Empty), new[]
        {
            nameof(FreightChargeTerms.Empty), nameof(FreightChargeTerms.Collect), nameof(FreightChargeTerms.Prepaid), nameof(FreightChargeTerms.ThirdParty)
        })]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_ChargeTerms = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_ChargeTerms);

        [ConfigDefinition(ConfigType.MultiSelect, "Default value for 'Freight Counted' option", nameof(FreightCountedType.Empty), new[]
        {
            nameof(FreightCountedType.Empty), nameof(FreightCountedType.ByShipper), nameof(FreightCountedType.ByDriverPallets), nameof(FreightCountedType.ByDriverPieces)
        })]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_FreightCounted = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_FreightCounted);

        [ConfigDefinition(ConfigType.MultiSelect, "Default value for 'Trailer Loaded' option", nameof(TrailerLoadedType.Empty), new[]
        {
            nameof(TrailerLoadedType.Empty), nameof(TrailerLoadedType.ByShipper),nameof(TrailerLoadedType.ByDriver)
        })]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_TrailerLoaded = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_TrailerLoaded);

        [ConfigDefinition(ConfigType.Bool, "Whether a Master BOL document should be automatically printed upon shipping", false)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_AutoPrintOnShip = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_AutoPrintOnShip);

        [ConfigDefinition(ConfigType.Bool, "Default value for 'Shipper e-sign' option", true)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_ShipperESign = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_ShipperESign);

        [ConfigDefinition(ConfigType.Bool, "Default value for 'Carrier e-sign' option", true)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_CarrierESign = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_CarrierESign);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Truck Loads to be assigned to dock doors for shipping", false)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_RequireDockDoor = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_RequireDockDoor);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Truck Loads to have Freight Charge terms setup before Staging", false)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_RequireFreightChargeTerms = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_RequireFreightChargeTerms);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Totes to be moved to Dock doors and require to match Dock door to Truck load dock door assignment", false)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_Operations_RequireDockDoorMatching = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_Operations_RequireDockDoorMatching);
        #endregion

        #region EmailNotification
        [ConfigDefinition(ConfigType.Bool, "Whether to send out emails on Closure of Truck loads", false)]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_SendEmail = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_SendEmail);

        [ConfigDefinition(ConfigType.String, "Comma separated list of recipients. Available keywords: (@CustomerEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_To = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_To);

        [ConfigDefinition(ConfigType.String, "Comma separated list of CC (Carbon Copy). Available keywords: (@CustomerEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Cc = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Cc);

        [ConfigDefinition(ConfigType.String, "Comma separated list of BCC (Blind Carbon Copy). Available keywords: (@CustomerEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Bcc = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Bcc);

        [ConfigDefinition(ConfigType.String, "Email Subject. Available keywords: (@TruckLoadNumber, @BillOfLadingNumber, @ShipDate, @CustomerName, @ClientName)")]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Subject = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Subject);

        [ConfigDefinition(ConfigType.EmailBody, "Email Body. Available keywords: (@TruckLoadNumber, @BillOfLadingNumber, @ShipDate, @CustomerName, @ClientName)")]
        public const string Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Body = nameof(Business_Fulfillment_Shipping_MasterTruckLoad_EmailNotification_Body);
        #endregion

        #endregion

        #region General
        [ConfigDefinition(ConfigType.String, "Tax ID for Commercial invoice generation purposes")]
        public const string Business_Fulfillment_Shipping_General_TaxId = nameof(Business_Fulfillment_Shipping_General_TaxId);

        [ConfigDefinition(ConfigType.String, "Contact name for Commercial invoice generation purposes")]
        public const string Business_Fulfillment_Shipping_General_ContactName = nameof(Business_Fulfillment_Shipping_General_ContactName);

        [ConfigDefinition(ConfigType.String, "Email for Commercial invoice generation purposes")]
        public const string Business_Fulfillment_Shipping_General_Email = nameof(Business_Fulfillment_Shipping_General_Email);
        #endregion

        #region Packslip
        [ConfigDefinition(ConfigType.Bool, "Whether to include photos of order on packslip report", false)]
        public const string Business_Fulfillment_Shipping_Packslip_IncludePhotosOnPackslip = nameof(Business_Fulfillment_Shipping_Packslip_IncludePhotosOnPackslip);

        [ConfigDefinition(ConfigType.Bool, "Whether a Packslip document should be automatically printed upon Shipping", false)]
        public const string Business_Fulfillment_Shipping_Packslip_AutoPrintOnShipping = nameof(Business_Fulfillment_Shipping_Packslip_AutoPrintOnShipping);

        [ConfigDefinition(ConfigType.Bool, "Whether a Packslip document should be automatically printed upon generating carrier label. Note! if this and 'Auto Print On Shipping' are both enabled, this will result in duplicated packslips", false)]
        public const string Business_Fulfillment_Shipping_Packslip_AutoPrintOnCarrtierLabel = nameof(Business_Fulfillment_Shipping_Packslip_AutoPrintOnCarrtierLabel);

        [ConfigDefinition(ConfigType.Bool, "Whether Pallet labels are required to be printed for a Packslip that has LPNs", false)]
        public const string Business_Fulfillment_Shipping_Packslip_RequirePalletPrinting = nameof(Business_Fulfillment_Shipping_Packslip_RequirePalletPrinting);
        #endregion

        #region Small Parcel
        [ConfigDefinition(ConfigType.MultiSelect, "Shipping service broker", nameof(ShippingBrokerType.EasyPost), new[] {nameof(ShippingBrokerType.EasyPost), nameof(ShippingBrokerType.Malvern)})]
        public const string Business_Fulfillment_Shipping_SmallParcel_ShippingBroker = nameof(Business_Fulfillment_Shipping_SmallParcel_ShippingBroker);

        [ConfigDefinition(ConfigType.String, "Shipping broker Url")]
        public const string Business_Fulfillment_Shipping_SmallParcel_BrokerUrl = nameof(Business_Fulfillment_Shipping_SmallParcel_BrokerUrl);

        [ConfigDefinition(ConfigType.String, "Shipping broker API Key")]
        public const string Business_Fulfillment_Shipping_SmallParcel_BrokerApiKey = nameof(Business_Fulfillment_Shipping_SmallParcel_BrokerApiKey);

        [ConfigDefinition(ConfigType.Bool, "Whether license plate to Carrier is required", false)]
        public const string Business_Fulfillment_Shipping_SmallParcel_RequireCarrierPalletAssignmant = nameof(Business_Fulfillment_Shipping_SmallParcel_RequireCarrierPalletAssignmant);

        [ConfigDefinition(ConfigType.Bool, "Allow re-shipping parcel with generated track number", false)]
        public const string Business_Fulfillment_Shipping_SmallParcel_AllowSmallParcelReShipping = nameof(Business_Fulfillment_Shipping_SmallParcel_AllowSmallParcelReShipping);

        [ConfigDefinition(ConfigType.MultilineString, "List of final carriers to be used for License Plate assignment")]
        public const string Business_Fulfillment_Shipping_SmallParcel_CarrierList = nameof(Business_Fulfillment_Shipping_SmallParcel_CarrierList);

        [ConfigDefinition(ConfigType.MultilineString, "Carriers and services map [<carrier1>=<service1>,<service2>;<carrier2>=<service3,service4>;]")]
        public const string Business_Fulfillment_Shipping_SmallParcel_CarrierServiceMap = nameof(Business_Fulfillment_Shipping_SmallParcel_CarrierServiceMap);
        
        [ConfigDefinition(ConfigType.Bool, "Whether a Packslip should be automatically shipped upon generating a carrier label. Otherwise a 'Carrier pickup' function would need to be used to mark order shipped.", true)]
        public const string Business_Fulfillment_Shipping_SmallParcel_AutoShipOnCarrierLabel = nameof(Business_Fulfillment_Shipping_SmallParcel_AutoShipOnCarrierLabel);

        [ConfigDefinition(ConfigType.String, "Carrier manifest prefix. Generate when totes get shipped picked up by a Small Parcel carrier", "CM-")]
        public const string Business_Fulfillment_Shipping_SmallParcel_CarrierManifestPrefix = nameof(Business_Fulfillment_Shipping_SmallParcel_CarrierManifestPrefix);

        [ConfigDefinition(ConfigType.MultiSelect, "Tote weight units of measure", nameof(UnitOfMeasure.Kg), new[]
        {
            nameof(UnitOfMeasure.Kg), nameof(UnitOfMeasure.Gr),
            nameof(UnitOfMeasure.Lb), nameof(UnitOfMeasure.Oz)
        })]
        public const string Business_Fulfillment_Shipping_SmallParcel_ToteWeightUnitOfMeasure = nameof(Business_Fulfillment_Shipping_SmallParcel_ToteWeightUnitOfMeasure);

        [ConfigDefinition(ConfigType.MultiSelect, "Tote length, width and height units of measure", nameof(UnitOfMeasure.Cm), new[]
        {
            nameof(UnitOfMeasure.M), nameof(UnitOfMeasure.Cm),nameof(UnitOfMeasure.Mm),
            nameof(UnitOfMeasure.Ft), nameof(UnitOfMeasure.In)
        })]
        public const string Business_Fulfillment_Shipping_SmallParcel_ToteLengthUnitOfMeasure = nameof(Business_Fulfillment_Shipping_SmallParcel_ToteLengthUnitOfMeasure);

        [ConfigDefinition(ConfigType.String, "Url to a instance of Tricolops for automated weight/dims capture")]
        public const string Business_Fulfillment_Shipping_SmallParcel_WeightDimsCaptureUrl = nameof(Business_Fulfillment_Shipping_SmallParcel_WeightDimsCaptureUrl);

        [ConfigDefinition(ConfigType.Bool, "Should a pick ticket be automatically updated to validated address if address validated is enabled/available and address is changed", false)]
        public const string Business_Fulfillment_Shipping_SmallParcel_UpdateToValidatedAddress = nameof(Business_Fulfillment_Shipping_SmallParcel_UpdateToValidatedAddress);

        [ConfigDefinition(ConfigType.Bool, "Whether a tote label should be printed with using E-com dashboard", false)]
        public const string Business_Fulfillment_Shipping_SmallParcel_EcomDashboardPrintToteLabel = nameof(Business_Fulfillment_Shipping_SmallParcel_EcomDashboardPrintToteLabel);
        #endregion

        #region OneScanShipping
        [ConfigDefinition(ConfigType.Bool, "Whenther One Scan Shipping requires Carton size selection [Checked] or Product level selection is allowed [Unchecked].", true)]
        public const string Business_Fulfillment_Shipping_OneScanShipping_RequireCartonSelection = nameof(Business_Fulfillment_Shipping_OneScanShipping_RequireCartonSelection);

        [ConfigDefinition(ConfigType.Bool, "Whenther One Scan Shipping requires weight to be entered [Checked] or Product level product weight is allowed [Unchecked].", true)]
        public const string Business_Fulfillment_Shipping_OneScanShipping_RequireWeight = nameof(Business_Fulfillment_Shipping_OneScanShipping_RequireWeight);

        [ConfigDefinition(ConfigType.Bool, "Whether One Scan Shipping requires orders to be pre-rated prior to shipping. Disabling this feature might impact performance of shipping", true)]
        public const string Business_Fulfillment_Shipping_OneScanShipping_RequirePreRating = nameof(Business_Fulfillment_Shipping_OneScanShipping_RequirePreRating);

        [ConfigDefinition(ConfigType.Bool, "Whether a tote label should be printed", false)]
        public const string Business_Fulfillment_Shipping_OneScanShipping_PrintToteLabel = nameof(Business_Fulfillment_Shipping_OneScanShipping_PrintToteLabel);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should be auto rated if it wasn't prerated. This setting works in tandem with 'Auto Purchase Carrier Label'", true)]
        public const string Business_Fulfillment_Shipping_OneScanShipping_AutoRateUnratedOrders = nameof(Business_Fulfillment_Shipping_OneScanShipping_AutoRateUnratedOrders);

        [ConfigDefinition(ConfigType.Bool, "Whether a carrier label should be purchased automatically", true)]
        public const string Business_Fulfillment_Shipping_OneScanShipping_AutoPurchaseCarrierLabel = nameof(Business_Fulfillment_Shipping_OneScanShipping_AutoPurchaseCarrierLabel);
        #endregion

        #region PrivateFleet
        [ConfigDefinition(ConfigType.Bool, "Whether to require Pick Tickets to be assigned to dock doors for shipping", false)]
        public const string Business_Fulfillment_Shipping_PrivateFleet_RequireDockDoor = nameof(Business_Fulfillment_Shipping_PrivateFleet_RequireDockDoor);

        [ConfigDefinition(ConfigType.Bool, "Whether to require Totes to be moved to Dock doors and require to match Dock door to Pick Ticket dock door assignment", false)]
        public const string Business_Fulfillment_Shipping_PrivateFleet_RequireDockDoorMatching = nameof(Business_Fulfillment_Shipping_PrivateFleet_RequireDockDoorMatching);

        [ConfigDefinition(ConfigType.String, "Default Driver Id to be used when Shipping. This value will be used during Auto Shipping")]
        public const string Business_Fulfillment_Shipping_PrivateFleet_DefaultDriverId = nameof(Business_Fulfillment_Shipping_PrivateFleet_DefaultDriverId);

        [ConfigDefinition(ConfigType.String, "Default Vehicle Id to be used when Shipping. This value will be used during Auto Shipping")]
        public const string Business_Fulfillment_Shipping_PrivateFleet_DefaultVehicleId = nameof(Business_Fulfillment_Shipping_PrivateFleet_DefaultVehicleId);

        [ConfigDefinition(ConfigType.String, "Default Seal number to be used when Shipping. This value will be used during Auto Shipping")]
        public const string Business_Fulfillment_Shipping_PrivateFleet_DefaultSealNumber = nameof(Business_Fulfillment_Shipping_PrivateFleet_DefaultSealNumber);

        [ConfigDefinition(ConfigType.Bool, "Track Pick ticket Geo Location", true)]
        public const string Business_Fulfillment_Shipping_PrivateFleet_TrackPickTicketGeoLocation = nameof(Business_Fulfillment_Shipping_PrivateFleet_TrackPickTicketGeoLocation);
        #endregion

        #region EmailNotification
        [ConfigDefinition(ConfigType.Bool, "Whether to send out emails on Shipping of Pick tickets", false)]
        public const string Business_Fulfillment_Shipping_EmailNotification_SendEmail = nameof(Business_Fulfillment_Shipping_EmailNotification_SendEmail);

        [ConfigDefinition(ConfigType.String, "Comma separated list of recipients. Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_EmailNotification_To = nameof(Business_Fulfillment_Shipping_EmailNotification_To);

        [ConfigDefinition(ConfigType.String, "Comma separated list of CC (Carbon Copy). Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_EmailNotification_Cc = nameof(Business_Fulfillment_Shipping_EmailNotification_Cc);

        [ConfigDefinition(ConfigType.String, "Comma separated list of BCC (Blind Carbon Copy). Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Shipping_EmailNotification_Bcc = nameof(Business_Fulfillment_Shipping_EmailNotification_Bcc);

        [ConfigDefinition(ConfigType.String, "Email Subject. Available keywords: (@PickTicketNumber, @CloseDate, @CustomerName, @ClientName)")]
        public const string Business_Fulfillment_Shipping_EmailNotification_Subject = nameof(Business_Fulfillment_Shipping_EmailNotification_Subject);

        [ConfigDefinition(ConfigType.EmailBody, "Email Body. Available keywords: (@PickTicketNumber, @CloseDate, @CustomerName, @ClientName, @TrackUrl)")]
        public const string Business_Fulfillment_Shipping_EmailNotification_Body = nameof(Business_Fulfillment_Shipping_EmailNotification_Body);
        #endregion
        
        #endregion

        #region Closing
        
        #region BackOrder
        [ConfigDefinition(ConfigType.Bool, "Whether to create a backorder on closing with outstanding quantities", false)]
        public const string Business_Fulfillment_Closing_BackOrders_GenerateBackOrderOnClose = nameof(Business_Fulfillment_Closing_BackOrders_GenerateBackOrderOnClose);

        [ConfigDefinition(ConfigType.Bool, "Whether to auto release backorders", false)]
        public const string Business_Fulfillment_Closing_BackOrders_AutoReleaseBackOrder = nameof(Business_Fulfillment_Closing_BackOrders_AutoReleaseBackOrder);
        #endregion

        #region EmailNotification
        [ConfigDefinition(ConfigType.Bool, "Whether to send out emails on Closing of Pick tickets", false)]
        public const string Business_Fulfillment_Closing_EmailNotification_SendEmail = nameof(Business_Fulfillment_Closing_EmailNotification_SendEmail);

        [ConfigDefinition(ConfigType.String, "Comma separated list of recipients. Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Closing_EmailNotification_To = nameof(Business_Fulfillment_Closing_EmailNotification_To);

        [ConfigDefinition(ConfigType.String, "Comma separated list of CC (Carbon Copy). Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Closing_EmailNotification_Cc = nameof(Business_Fulfillment_Closing_EmailNotification_Cc);

        [ConfigDefinition(ConfigType.String, "Comma separated list of BCC (Blind Carbon Copy). Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Fulfillment_Closing_EmailNotification_Bcc = nameof(Business_Fulfillment_Closing_EmailNotification_Bcc);

        [ConfigDefinition(ConfigType.String, "Email Subject. Available keywords: (@PickTicketNumber, @CloseDate, @CustomerName, @ClientName)")]
        public const string Business_Fulfillment_Closing_EmailNotification_Subject = nameof(Business_Fulfillment_Closing_EmailNotification_Subject);

        [ConfigDefinition(ConfigType.EmailBody, "Email Body. Available keywords: (@PickTicketNumber, @CloseDate, @CustomerName, @ClientName, @TrackUrl)")]
        public const string Business_Fulfillment_Closing_EmailNotification_Body = nameof(Business_Fulfillment_Closing_EmailNotification_Body);
        #endregion

        #endregion

        #region Data entry
        [ConfigDefinition(ConfigType.String, "Pick ticket number prefix", "PCK-")]
        public const string Business_Fulfillment_DataEntry_PickTicketNumberPrefix = nameof(Business_Fulfillment_DataEntry_PickTicketNumberPrefix);

        [ConfigDefinition(ConfigType.Bool, "Whether a custom WO number could be entered during data entry", true)]
        public const string Business_Fulfillment_DataEntry_AllowCustomPickTicketNumber = nameof(Business_Fulfillment_DataEntry_AllowCustomPickTicketNumber);

        [ConfigDefinition(ConfigType.String, "Warehouse transfer number prefix", "TRNS-")]
        public const string Business_Fulfillment_DataEntry_WarehouseTransferNumberPrefix = nameof(Business_Fulfillment_DataEntry_WarehouseTransferNumberPrefix);
        
        [ConfigDefinition(ConfigType.String, "Shipping rule prefix", "RL-")]
        public const string Business_Fulfillment_DataEntry_ShippingRulePrefix = nameof(Business_Fulfillment_DataEntry_ShippingRulePrefix);

        [ConfigDefinition(ConfigType.MultiSelect, "Data entry unit of measure for packsize controlled items", "Packs", new []{ "Packs", "Eaches" })]
        public const string Business_Fulfillment_DataEntry_PacksizeEntryType = nameof(Business_Fulfillment_DataEntry_PacksizeEntryType);

        [ConfigDefinition(ConfigType.MultiSelect, "Duplicate SKUs line behaviour. Allow - allows duplicates, Warning - Will present a prompt warning of a duplicate. Prevent - will error on attempt to create a duplicate.", nameof(DuplicateLineSkuBehaviour.Allow),
            new[] { nameof(DuplicateLineSkuBehaviour.Allow), nameof(DuplicateLineSkuBehaviour.Prompt), nameof(DuplicateLineSkuBehaviour.Prevent) })]
        public const string Business_Fulfillment_DataEntry_DuplicateSkuBehaviour = nameof(Business_Fulfillment_DataEntry_DuplicateSkuBehaviour);

        [ConfigDefinition(ConfigType.String, "Comma separated list of fields that are available for Apply Field(s) function in Pick ticket list", "")]
        public const string Business_Fulfillment_DataEntry_BulkUpdateFields = nameof(Business_Fulfillment_DataEntry_BulkUpdateFields);
        #endregion

        #region Automation
        [ConfigDefinition(ConfigType.Bool, "Whether to reset user assignment throught the lifecycle of an order", false)]
        public const string Business_Fulfillment_Automation_ResetUserOnChange = nameof(Business_Fulfillment_Automation_ResetUserOnChange);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto allocate upon being released to floor. Default allocation settings will be used", false)]
        public const string Business_Fulfillment_Automation_AutoAllocateOnReleaseToFloor = nameof(Business_Fulfillment_Automation_AutoAllocateOnReleaseToFloor);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto cartonize on allocation using customer profile and default settings", false)]
        public const string Business_Fulfillment_Automation_AutoCartonizeOnAllocation = nameof(Business_Fulfillment_Automation_AutoCartonizeOnAllocation);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto wave on allocation. Default waving settings will be used", false)]
        public const string Business_Fulfillment_Automation_AutoWaveOnAllocation = nameof(Business_Fulfillment_Automation_AutoWaveOnAllocation);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto wave on production. Default waving settings will be used", false)]
        public const string Business_Fulfillment_Automation_AutoWaveOnProduction = nameof(Business_Fulfillment_Automation_AutoWaveOnProduction);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto ship on last pick. If Proof of shipment is required, this option will execute when Shipped qty is equal to Picked quantity", false)]
        public const string Business_Fulfillment_Automation_AutoShipOnLastPick = nameof(Business_Fulfillment_Automation_AutoShipOnLastPick);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto suspended on last pick if order was Handheld shorted", false)]
        public const string Business_Fulfillment_Automation_SuspendShortedOnLastPick = nameof(Business_Fulfillment_Automation_SuspendShortedOnLastPick);

        [ConfigDefinition(ConfigType.Bool, "Whether an order should auto close upon shipping. If Proof of delivery is required, this option will execute when Delivered qty is equal to Shipped quantity", false)]
        public const string Business_Fulfillment_Automation_AutoCloseOnShip = nameof(Business_Fulfillment_Automation_AutoCloseOnShip);
        #endregion

        #region Releasing to floor
        [ConfigDefinition(ConfigType.Bool, "Whether an address should be validated and Geo Coded upon releasing to floor - Does not work with TruckLoad consolidation. Note, address may change after geo coding. Note, Additional costs may apply", false)]
        public const string Business_Fulfillment_Releasing_EnableAddressGeoCoding = nameof(Business_Fulfillment_Releasing_EnableAddressGeoCoding);

        [ConfigDefinition(ConfigType.Int, "Address preview zoom", 10)]
        public const string Business_Fulfillment_Releasing_PreviewZoom = nameof(Business_Fulfillment_Releasing_PreviewZoom);

        [ConfigDefinition(ConfigType.Bool, "Whether one line Small Parcel orders should be pre-rated on release. A default Carton Size must be setup on products for pre-rating to work", false)]
        public const string Business_Fulfillment_Releasing_PreRateOneLineOrders = nameof(Business_Fulfillment_Releasing_PreRateOneLineOrders);
        #endregion

        #region Allocation
        [ConfigDefinition(ConfigType.String, "Comma separated list of zones to include in allocation. Ex: 'A1,B2,C3'")]
        public const string Business_Fulfillment_Allocation_ZonesToInclude = nameof(Business_Fulfillment_Allocation_ZonesToInclude);

        [ConfigDefinition(ConfigType.MultiSelect, "Options for allocation behaviour. FIFO - First in, first out. LIFO - Last in, first out. BinName - Based on alphanumeric bin sorting. MostProductFirst - Attempt to consume product from bin/lpn with most quantity first. LeastProductFirst - Attempt to consume product from bin/lpn with least quantity first", nameof(AllocationStyleEnum.FIFO),
            new[] { nameof(AllocationStyleEnum.FIFO), nameof(AllocationStyleEnum.LIFO), nameof(AllocationStyleEnum.BinName), nameof(AllocationStyleEnum.MostProductFirst), nameof(AllocationStyleEnum.LeastProductFirst) })]
        public const string Business_Fulfillment_Allocation_AllocationStyle = nameof(Business_Fulfillment_Allocation_AllocationStyle);

        [ConfigDefinition(ConfigType.MultiSelect, "Options for short behaviour. PickShort - Will allow shipping shorted orders. HoldShort - Does not releases order for picking but reserves the stock", nameof(ShortOptionsEnum.HoldShort),
            new[] { nameof(ShortOptionsEnum.PickShort), nameof(ShortOptionsEnum.HoldShort) })]
        public const string Business_Fulfillment_Allocation_ShortOptions = nameof(Business_Fulfillment_Allocation_ShortOptions);

        [ConfigDefinition(ConfigType.MultiSelect, "Expiry controlled items override allocation style", nameof(ExpiryAllocationPrecedenceEnum.NoPrecedence), 
            new[] { nameof(ExpiryAllocationPrecedenceEnum.NoPrecedence), nameof(ExpiryAllocationPrecedenceEnum.OldestFirst), nameof(ExpiryAllocationPrecedenceEnum.NewestFirst) })]
        public const string Business_Fulfillment_Allocation_ExpiryAllocationPrecedence = nameof(Business_Fulfillment_Allocation_ExpiryAllocationPrecedence);

        [ConfigDefinition(ConfigType.Int, "Minimum number of days to be allowed on an expiry product to be allocated. This option is overriden by a customer setting", 45)]
        public const string Business_Fulfillment_Allocation_ExpiryAllowance = nameof(Business_Fulfillment_Allocation_ExpiryAllowance);

        [ConfigDefinition(ConfigType.MultiSelect, "Lot controlled items allocation style", nameof(LotAllocationStyleEnum.Inherit), 
            new[] { nameof(LotAllocationStyleEnum.Inherit), nameof(LotAllocationStyleEnum.Alphabetical), nameof(LotAllocationStyleEnum.MaxQtyFirst), nameof(LotAllocationStyleEnum.MinQtyFirst) })]
        public const string Business_Fulfillment_Allocation_LotAllocationStyle = nameof(Business_Fulfillment_Allocation_LotAllocationStyle);

        [ConfigDefinition(ConfigType.Bool, "Allow partial lot allocation. If enabled, the system will allow multiple lots to be picked.", true)]
        public const string Business_Fulfillment_Allocation_AllowPartialLots = nameof(Business_Fulfillment_Allocation_AllowPartialLots);

        [ConfigDefinition(ConfigType.Bool, "Whether zone sequence setup on Allocation screen should be enforced during allocation (ex: Zones A, B, C will enforce stock to be first allocated from zone A then B then C)", false)]
        public const string Business_Fulfillment_Allocation_ObeyZoneSequence = nameof(Business_Fulfillment_Allocation_ObeyZoneSequence);

        [ConfigDefinition(ConfigType.Bool, "Include LPNs in Bulk Zones during allocation to pick/replenish from", true)]
        public const string Business_Fulfillment_Allocation_AllocateBulk = nameof(Business_Fulfillment_Allocation_AllocateBulk);

        [ConfigDefinition(ConfigType.Bool, "Whether to hold order for Letdown", true)]
        public const string Business_Fulfillment_Allocation_HoldLetdown = nameof(Business_Fulfillment_Allocation_HoldLetdown);

        [ConfigDefinition(ConfigType.MultiSelect, "Packsize allocation style", nameof(PacksizeAllocationStyleEnum.Inherit),
            new[] { nameof(PacksizeAllocationStyleEnum.Inherit), nameof(PacksizeAllocationStyleEnum.BiggerPacksizeFirst), nameof(PacksizeAllocationStyleEnum.SmallerPacksizeFirst) })]
        public const string Business_Fulfillment_Allocation_PacksizeAllocationStyle = nameof(Business_Fulfillment_Allocation_PacksizeAllocationStyle);

        [ConfigDefinition(ConfigType.Bool, "Whether to issue packsize breakdown requests and hold order", true)]
        public const string Business_Fulfillment_Allocation_PacksizeBreakdown = nameof(Business_Fulfillment_Allocation_PacksizeBreakdown);

        [ConfigDefinition(ConfigType.Bool, "Whether to allow allocating eaches for packsize controlled items", true)]
        public const string Business_Fulfillment_Allocation_AllocateEaches = nameof(Business_Fulfillment_Allocation_AllocateEaches);

        [ConfigDefinition(ConfigType.Bool, "Whether to break packsizes to inner packs", false)]
        public const string Business_Fulfillment_Allocation_BreakToInnerPacks = nameof(Business_Fulfillment_Allocation_BreakToInnerPacks);

        [ConfigDefinition(ConfigType.Bool, "Allow allocating orders with overdue Cancel date", false)]
        public const string Business_Fulfillment_Allocation_AllocateCancelledOverdue = nameof(Business_Fulfillment_Allocation_AllocateCancelledOverdue);

        [ConfigDefinition(ConfigType.Bool, "Generate production for missing BOM products", false)]
        public const string Business_Fulfillment_Allocation_GenerateProduction = nameof(Business_Fulfillment_Allocation_GenerateProduction);

        [ConfigDefinition(ConfigType.Bool, "Auto release generated production", false)]
        public const string Business_Fulfillment_Allocation_AutoReleaseProduction = nameof(Business_Fulfillment_Allocation_AutoReleaseProduction);

        [ConfigDefinition(ConfigType.MultiSelect, "What to do with Production bound to this PickTicket on Unallocation. [PreventUnallocation] - Prevents UnAllocation, production needs to be cleared manually. [Delete] - Production will be attempted to delete, Note: Production must be in Draft or Unallocated to be deleted. [Detach] - Production will be detached from this Pick ticket", 
            nameof(UnAllocateProductionAction.Delete), new[]{ nameof(UnAllocateProductionAction.Detach), nameof(UnAllocateProductionAction.Delete) })]
        public const string Business_Fulfillment_Allocation_ProductionOnUnallocate = nameof(Business_Fulfillment_Allocation_ProductionOnUnallocate);
        #endregion

        #region Cartonization
        [ConfigDefinition(ConfigType.Bool, "Enable cartonizartion", false)]
        public const string Business_Fulfillment_Cartonization_Enable = nameof(Business_Fulfillment_Cartonization_Enable);

        [ConfigDefinition(ConfigType.String, "Default cartonization profile name. Will be used for warehouse transfers and if customer does not have one setup explicitly")]
        public const string Business_Fulfillment_Cartonization_DefaultProfile = nameof(Business_Fulfillment_Cartonization_DefaultProfile);

        //[ConfigDefinition(ConfigType.MultiSelect, "Tote default weight units of measure", nameof(UnitOfMeasure.Kg), new[]
        //{
        //    nameof(UnitOfMeasure.Kg), nameof(UnitOfMeasure.Gr),
        //    nameof(UnitOfMeasure.Lb), nameof(UnitOfMeasure.Oz)
        //})]
        //public const string Business_Fulfillment_Cartonization_ToteWeightUnitOfMeasure = nameof(Business_Fulfillment_Cartonization_ToteWeightUnitOfMeasure);

        //[ConfigDefinition(ConfigType.MultiSelect, "Tote default length, width and height units of measure", nameof(UnitOfMeasure.Cm), new[]
        //{
        //    nameof(UnitOfMeasure.M), nameof(UnitOfMeasure.Cm),nameof(UnitOfMeasure.Mm),
        //    nameof(UnitOfMeasure.Ft), nameof(UnitOfMeasure.In)
        //})]
        //public const string Business_Fulfillment_Cartonization_ToteLengthUnitOfMeasure = nameof(Business_Fulfillment_Cartonization_ToteLengthUnitOfMeasure);
        #endregion

        #region Waving
        [ConfigDefinition(ConfigType.Bool, "Whether labels should be printed during waving. Alternatively, labels could be reprinted from other screens (ex: Tote list)", true)]
        public const string Business_Fulfillment_Waving_PrintLabels = nameof(Business_Fulfillment_Waving_PrintLabels);

        [ConfigDefinition(ConfigType.Bool, "Whether a carton content label required on all totes. Note! this will require all pick tickets to be cartonized prior to Waving.", false)]
        public const string Business_Fulfillment_Waving_CartonContentRequired = nameof(Business_Fulfillment_Waving_CartonContentRequired);

        [ConfigDefinition(ConfigType.MultiSelect, "Default Packaging type for SSCC-18 generation", nameof(SsccTypeEnum.CaseOrCarton), 
            new[] { nameof(SsccTypeEnum.CaseOrCarton), nameof(SsccTypeEnum.Pallet), nameof(SsccTypeEnum.IntraCompanyUse), nameof(SsccTypeEnum.Undefined) })]
        public const string Business_Fulfillment_Waving_SsccPackagingType = nameof(Business_Fulfillment_Waving_SsccPackagingType);

        [ConfigDefinition(ConfigType.String, "GS1 Company Prefix [7 digits]. Used in SSCC-18 generation unless one setup on a 3PL client", "0000000")]
        public const string Business_Fulfillment_Waving_SsccGlobalCompanyId = nameof(Business_Fulfillment_Waving_SsccGlobalCompanyId);

        [ConfigDefinition(ConfigType.String, "Wave number prefix", "WV")]
        public const string Business_Fulfillment_Waving_WaveNumberPrefix = nameof(Business_Fulfillment_Waving_WaveNumberPrefix);

        [ConfigDefinition(ConfigType.Bool, "Whether multi zone waving is enabled. If selected, a tote label will be printed for each zone to a zone defined printer", false)]
        public const string Business_Fulfillment_Waving_MultiZoneWaving = nameof(Business_Fulfillment_Waving_MultiZoneWaving);

        [ConfigDefinition(ConfigType.String, "Name of the Carton size to be used during manual waving without cartonization. Also this carton will be used when printing from the handheld")]
        public const string Business_Fulfillment_Waving_DefaultCartonSizeName = nameof(Business_Fulfillment_Waving_DefaultCartonSizeName);

        [ConfigDefinition(ConfigType.MultiSelect, "Type of picking tote. Carton - Can be staged into a Pickable bin. Pallet can be staged into a Bulk bin. Other cannot be staged at all", nameof(ToteType.Carton), new[]
        {
            nameof(ToteType.Carton),nameof(ToteType.Pallet),nameof(ToteType.Other)
        })]
        public const string Business_Fulfillment_Waving_DefaultToteType = nameof(Business_Fulfillment_Waving_DefaultToteType);

        [ConfigDefinition(ConfigType.Int, "Number of cartons to generate per Pick ticket", 1)]
        public const string Business_Fulfillment_Waving_NumberOfCartons = nameof(Business_Fulfillment_Waving_NumberOfCartons);

        #endregion

        #region Picking
        [ConfigDefinition(ConfigType.Bool, "Whether to allow scanning Full Pallet/LPN instead of scanning contents of an LPN when that LPN is fully allocated to an order", false)]
        public const string Business_Fulfillment_Picking_AllowFullPalletPicking = nameof(Business_Fulfillment_Picking_AllowFullPalletPicking);

        [ConfigDefinition(ConfigType.Bool, "Allow Auto picking. WARNING!!! This will automatically reduce inventory without physical scans. This operation should only be used during training or by trained personnel in special circumstances!", false)]
        public const string Business_Fulfillment_Picking_AllowAutoPicking = nameof(Business_Fulfillment_Picking_AllowAutoPicking);

        [ConfigDefinition(ConfigType.Int, "Maximum quantity for a single pick when doing Pack picking. WARNING!!! - Making this value too large may cause severe performance degradation", 200)]
        public const string Business_Fulfillment_Picking_FullPackPickingQuantityLimit = nameof(Business_Fulfillment_Picking_FullPackPickingQuantityLimit);

        [ConfigDefinition(ConfigType.Int, "Maximum order quantity on an order to allow full pack picking. WARNING!!! - Making this value too large may cause severe performance degradation", 5000)]
        public const string Business_Fulfillment_Picking_FullPackPickingOrderQuantityLimit = nameof(Business_Fulfillment_Picking_FullPackPickingOrderQuantityLimit);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will allow non administrator users to short picks", true)]
        public const string Business_Fulfillment_Picking_AllowShorting = nameof(Business_Fulfillment_Picking_AllowShorting);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will allow users to skip picks", false)]
        public const string Business_Fulfillment_Picking_AllowSkipping = nameof(Business_Fulfillment_Picking_AllowSkipping);

        [ConfigDefinition(ConfigType.Bool, "Mark for Cycle count on Shorting", true)]
        public const string Business_Fulfillment_Picking_MarkForCycleCountOnShort = nameof(Business_Fulfillment_Picking_MarkForCycleCountOnShort);

        [ConfigDefinition(ConfigType.Bool, "Ask if bin is empty on picking last product from the bin", false)]
        public const string Business_Fulfillment_Picking_EmptyBinPrompt = nameof(Business_Fulfillment_Picking_EmptyBinPrompt);

        [ConfigDefinition(ConfigType.Bool, "Whether to enforce picking to follow cartonization results. If set to false, picker may be able to override cartonization results during picking by picking to alternate totes", false)]
        public const string Business_Fulfillment_Picking_EnforceCartonization = nameof(Business_Fulfillment_Picking_EnforceCartonization);

        [ConfigDefinition(ConfigType.Bool, "Whether to enforce customer cartonization profile rules. Ex: if cartonization dictates no more than 5 units per box, picking more than 5 units will not be allowed", true)]
        public const string Business_Fulfillment_Picking_EnforceCustomerProfile = nameof(Business_Fulfillment_Picking_EnforceCustomerProfile);

        [ConfigDefinition(ConfigType.Bool, "Whether to enforce carton size limitation. If set, pickers will not be allowed to pack more product than carton dimensions/weight allows", false)]
        public const string Business_Fulfillment_Picking_EnforceCartonCubeWeight = nameof(Business_Fulfillment_Picking_EnforceCartonCubeWeight);
        #endregion

        #region DocumentPrinting
        [ConfigDefinition(ConfigType.MultilineString, "Default message that will be printed on Packslip document (PDF) in the Footer section. Typically contains either Disclaimer information or Returns instructions. Could be overriden on Client or Partner level")]
        public const string Business_Fulfillment_DocumentPrinting_PackslipDisclaimer = nameof(Business_Fulfillment_DocumentPrinting_PackslipDisclaimer);

        [ConfigDefinition(ConfigType.MultilineString, "Default message that will be printed on Proforma Invoice document (PDF) in the Footer section. Typically contains Disclaimer information. Could be overriden on Client or Partner level")]
        public const string Business_Fulfillment_DocumentPrinting_ProformaInvoiceDisclaimer = nameof(Business_Fulfillment_DocumentPrinting_ProformaInvoiceDisclaimer);
        #endregion

        [ConfigDefinition(ConfigType.Bool, "Whether to automatically Wave pick ticket when PickTicket number is either scanned or selected from the list", true)]
        public const string Business_Fulfillment_Handheld_AutoWavePickTicketOnScan = nameof(Business_Fulfillment_Handheld_AutoWavePickTicketOnScan);

        [ConfigDefinition(ConfigType.MultiSelect, "Which tag to use in Tag List selection handheld screen", "Tag1", new[]{"Tag1", "Tag2" , "Tag3" , "Tag4" , "Tag5" })]
        public const string Business_Fulfillment_Handheld_TagList = nameof(Business_Fulfillment_Handheld_TagList);

        #endregion

        #region Returns

        #region DataEntry
        [ConfigDefinition(ConfigType.String, "Customer return number prefix", "RMA-")]
        public const string Business_Returns_DataEntry_CustomerReturnNumberPrefix = nameof(Business_Returns_DataEntry_CustomerReturnNumberPrefix);

        [ConfigDefinition(ConfigType.Bool, "Whether a custom RMA number could be entered during data entry", true)]
        public const string Business_Returns_DataEntry_AllowCustomRmaNumber = nameof(Business_Returns_DataEntry_AllowCustomRmaNumber);

        [ConfigDefinition(ConfigType.MultiSelect, "Duplicate SKUs line behaviour. Allow - allows duplicates, Warning - Will present a prompt warning of a duplicate. Prevent - will error on attempt to create a duplicate.", nameof(DuplicateLineSkuBehaviour.Allow),
            new[] { nameof(DuplicateLineSkuBehaviour.Allow), nameof(DuplicateLineSkuBehaviour.Prompt), nameof(DuplicateLineSkuBehaviour.Prevent) })]
        public const string Business_Returns_DataEntry_DuplicateSkuBehaviour = nameof(Business_Returns_DataEntry_DuplicateSkuBehaviour);
        #endregion

        #region Operations
        [ConfigDefinition(ConfigType.Bool, "Whether a Customer return should be allowed to receive without assigned Operator", true)]
        public const string Business_Returns_Operations_AllowReceivingWithoutAssignment = nameof(Business_Returns_Operations_AllowReceivingWithoutAssignment);

        [ConfigDefinition(ConfigType.Bool, "Whether a Customer return should be allowed to receive without Dock door assigned", true)]
        public const string Business_Returns_Operations_AllowReceivingWithoutDockDoor = nameof(Business_Returns_Operations_AllowReceivingWithoutDockDoor);

        [ConfigDefinition(ConfigType.Int, "Whether to allow expired product and expiry allowance in days. Ex: 5 mean admitting product will expire in 5 days, any value less than that will be rejected. Ex: -5 admitting product has expired for 5 days, any value that has been expired for more than 5 days will be rejected", 0)]
        public const string Business_Returns_Operations_ExpiryAllowance = nameof(Business_Returns_Operations_ExpiryAllowance);

        [ConfigDefinition(ConfigType.Int, "Over-Receive threshold. How much more of the product could be received (in %) compared to ordered quantity. Zero means - no over receiving will be accepted", 0)]
        public const string Business_Returns_Operations_OverReceiveThreshold = nameof(Business_Returns_Operations_OverReceiveThreshold);

        [ConfigDefinition(ConfigType.Bool, "Whether to include photos on receiving slip report", false)]
        public const string Business_Returns_Operations_IncludePhotosOnRecvSlip = nameof(Business_Returns_Operations_IncludePhotosOnRecvSlip);

        [ConfigDefinition(ConfigType.Bool, "Whether it is allowed to discard damaged products during receiving", false)]
        public const string Business_Returns_Operations_AllowDiscardingDamagedProduct = nameof(Business_Returns_Operations_AllowDiscardingDamagedProduct);
        #endregion

        #region Handheld
        [ConfigDefinition(ConfigType.Bool, "Whether to show expected quantity during receiving", true)]
        public const string Business_Returns_Handheld_ShowExpectedQuantity = nameof(Business_Returns_Handheld_ShowExpectedQuantity);

        [ConfigDefinition(ConfigType.Bool, "Whether a Handheld operator should be allowed to close Return document", true)]
        public const string Business_Returns_Handheld_AllowHandheldClose = nameof(Business_Returns_Handheld_AllowHandheldClose);

        [ConfigDefinition(ConfigType.Bool, "Print labels during receiving", false)]
        public const string Business_Returns_Handheld_PrintLabels = nameof(Business_Returns_Handheld_PrintLabels);

        [ConfigDefinition(ConfigType.Bool, "Whether the system should suggest a bin to receive to", false)]
        public const string Business_Returns_Handheld_SuggestPutawayBin = nameof(Business_Returns_Handheld_SuggestPutawayBin);
        #endregion

        #region Automation
        [ConfigDefinition(ConfigType.Bool, "Whether to close Returns automatically upon receipt", true)]
        public const string Business_Returns_Automation_AutoCloseReceivedReturns = nameof(Business_Returns_Automation_AutoCloseReceivedReturns);

        [ConfigDefinition(ConfigType.Bool, "Whether to auto release backorders", false)]
        public const string Business_Returns_Automation_AutoReleaseBackOrder = nameof(Business_Returns_Automation_AutoReleaseBackOrder);
        #endregion

        #region DocumentPrinting
        [ConfigDefinition(ConfigType.MultilineString, "Default message that will be printed on Returns document (PDF) in the Footer section. Typically contains Disclaimer. Could be overriden on Client or Partner level")]
        public const string Business_Returns_DocumentPrinting_ReturnsDisclaimer = nameof(Business_Returns_DocumentPrinting_ReturnsDisclaimer);
        #endregion

        #region EmailNotification
        [ConfigDefinition(ConfigType.Bool, "Whether to send out emails on Closure of Customer return", false)]
        public const string Business_Returns_EmailNotification_SendEmail = nameof(Business_Returns_EmailNotification_SendEmail);

        [ConfigDefinition(ConfigType.String, "Comma separated list of recipients. Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Returns_EmailNotification_To = nameof(Business_Returns_EmailNotification_To);

        [ConfigDefinition(ConfigType.String, "Comma separated list of CC (Carbon Copy). Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Returns_EmailNotification_Cc = nameof(Business_Returns_EmailNotification_Cc);

        [ConfigDefinition(ConfigType.String, "Comma separated list of BCC (Blind Carbon Copy). Available keywords: (@CustomerEmail, @ShipToEmail, @ClientEmail)")]
        public const string Business_Returns_EmailNotification_Bcc = nameof(Business_Returns_EmailNotification_Bcc);

        [ConfigDefinition(ConfigType.MultilineString, "Email Subject. Available keywords: (@CustomerReturnNumber, @ReceiveDate, @CustomerName, @ClientName)")]
        public const string Business_Returns_EmailNotification_Subject = nameof(Business_Returns_EmailNotification_Subject);

        [ConfigDefinition(ConfigType.EmailBody, "Email Body. Available keywords: (@CustomerReturnNumber, @ReceiveDate, @CustomerName, @ClientName)")]
        public const string Business_Returns_EmailNotification_Body = nameof(Business_Returns_EmailNotification_Body);
        #endregion

        #endregion

        #region Handheld
        //General
        [ConfigDefinition(ConfigType.Bool, "Whether scanning quantity is allowed. Alternatively an operator needs to enter value manually", false)]
        public const string Business_Handheld_AllowQuantityScan = nameof(Business_Handheld_AllowQuantityScan);

        [ConfigDefinition(ConfigType.Bool, "Whether scanning dates is allowed. Alternatively an operator needs to enter value manually", true)]
        public const string Business_Handheld_AllowDateScan = nameof(Business_Handheld_AllowDateScan);

        [ConfigDefinition(ConfigType.MultiSelect, "If enabled, will force barcode product scan, otherwise SKU could be entered manually", nameof(ScanType.ZebraDataWedge), 
            new []{ nameof(ScanType.ZebraDataWedge), nameof(ScanType.Camera), nameof(ScanType.LineFeed), nameof(ScanType.UserSpecific) }, false)]
        public const string Business_Handheld_ScanType = nameof(Business_Handheld_ScanType);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force barcode product scan, otherwise SKU could be entered manually", true)]
        public const string Business_Handheld_ForceProductScan = nameof(Business_Handheld_ForceProductScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan Bin/LPN, otherwise could be entered manually", true)]
        public const string Business_Handheld_ForceBinLpnScan = nameof(Business_Handheld_ForceBinLpnScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan DockDoor, otherwise could be entered manually", true)]
        public const string Business_Handheld_ForceDockDoorScan = nameof(Business_Handheld_ForceDockDoorScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan a PO, otherwise PO could be entered manually", true)]
        public const string Business_Handheld_ForcePoScan = nameof(Business_Handheld_ForcePoScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan Return number, otherwise return number could be entered manually", true)]
        public const string Business_Handheld_ForceCustomerReturnScan = nameof(Business_Handheld_ForceCustomerReturnScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan a Pick ticket, otherwise Pick ticket could be entered manually", true)]
        public const string Business_Handheld_ForcePickTicketScan = nameof(Business_Handheld_ForcePickTicketScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan a Truck Load or BOL number, otherwise it could be entered manually", true)]
        public const string Business_Handheld_ForceTruckLoadBolScan = nameof(Business_Handheld_ForceTruckLoadBolScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan a Work order, otherwise Work order could be entered manually", true)]
        public const string Business_Handheld_ForceWoScan = nameof(Business_Handheld_ForceWoScan);

        [ConfigDefinition(ConfigType.Bool, "If enabled, will force operator to scan a sroduction order, otherwise Production order could be entered manually", true)]
        public const string Business_Handheld_ForceProductionOrderScan = nameof(Business_Handheld_ForceProductionOrderScan);
        
        [ConfigDefinition(ConfigType.Bool, "Display product locations on Scan during product moves and letdowns. Only Pickable (no Overstock or Quarantine or Production) locations will be shown. This option will supercede [Suggest Product Move Bin] setting", false)]
        public const string Business_Handheld_ProductLocationOnMove = nameof(Business_Handheld_ProductLocationOnMove);

        [ConfigDefinition(ConfigType.Bool, "Whether the system should suggest a bin during directed move. This option should not be used together with [Product Location On Move] setting and if used together, will be ignored.", false)]
        public const string Business_Handheld_SuggestProductMoveBin = nameof(Business_Handheld_SuggestProductMoveBin);
        #endregion

        //Operations
        #region Products
        [ConfigDefinition(ConfigType.Int, "Maximum number of lines to be printed per content label page. Max 30", 15)]
        public const string Business_Products_ContentLabelPageSize = nameof(Business_Products_ContentLabelPageSize);

        [ConfigDefinition(ConfigType.String, "Expiry date format", "yyyyMMdd")]
        public const string Business_Products_ExpiryFormat = nameof(Business_Products_ExpiryFormat);

        [ConfigDefinition(ConfigType.Bool, "Allow receiving and adjusting expired product", false)]
        public const string Business_Products_AllowExpiredProduct = nameof(Business_Products_AllowExpiredProduct);

        [ConfigDefinition(ConfigType.Bool, "Print Packsize labels when consolidating Packsizes", true)]
        public const string Business_Products_PrintPacksizeLabelsOnConsolidate = nameof(Business_Products_PrintPacksizeLabelsOnConsolidate);

        [ConfigDefinition(ConfigType.Double, "Acceptable error margin (in UoM) when handling Decimal controlled product", 1.5)]
        public const string Business_Products_DecimalErrorMargin = nameof(Business_Products_DecimalErrorMargin);

        [ConfigDefinition(ConfigType.MultiSelect, "Default weight unit of measure. Used in Reports and weight capture HH screens", nameof(UnitOfMeasure.Kg), new[]
        {
            nameof(UnitOfMeasure.Kg), nameof(UnitOfMeasure.Gr),
            nameof(UnitOfMeasure.Lb), nameof(UnitOfMeasure.Oz)
        })]
        public const string Business_Products_ReportWeightUnitOfMeasure = nameof(Business_Products_ReportWeightUnitOfMeasure);

        [ConfigDefinition(ConfigType.MultiSelect, "Default length/width/height unit of measure. Used in Reports and weight capture HH screens", nameof(UnitOfMeasure.M), new[]
        {
            nameof(UnitOfMeasure.M), nameof(UnitOfMeasure.Cm), nameof(UnitOfMeasure.Mm),
            nameof(UnitOfMeasure.Ft), nameof(UnitOfMeasure.In)
        })]
        public const string Business_Products_ReportLengthUnitOfMeasure = nameof(Business_Products_ReportLengthUnitOfMeasure);

        [ConfigDefinition(ConfigType.MultiSelect, "Default weight unit of measure. Used in weight capture HH screens", nameof(UnitOfMeasure.Kg), new[]
        {
            nameof(UnitOfMeasure.Kg), nameof(UnitOfMeasure.Gr),
            nameof(UnitOfMeasure.Lb), nameof(UnitOfMeasure.Oz)
        })]
        public const string Business_Products_CaptureWeightUnitOfMeasure = nameof(Business_Products_CaptureWeightUnitOfMeasure);

        [ConfigDefinition(ConfigType.MultiSelect, "Default length/width/height unit of measure. Used in Dims capture HH screens", nameof(UnitOfMeasure.M), new[]
        {
            nameof(UnitOfMeasure.M), nameof(UnitOfMeasure.Cm), nameof(UnitOfMeasure.Mm),
            nameof(UnitOfMeasure.Ft), nameof(UnitOfMeasure.In)
        })]
        public const string Business_Products_CaptureLengthUnitOfMeasure = nameof(Business_Products_CaptureLengthUnitOfMeasure);

        [ConfigDefinition(ConfigType.Bool, "Whether attribute control can be changed for products that have transactions", false)]
        public const string Business_Products_AllowAttributeChange = nameof(Business_Products_AllowAttributeChange);

        [ConfigDefinition(ConfigType.Bool, "Whether Lot numbers should be cleansed of leading/trailing white spaces before comitting to a database. Please note that if Lots are being scanned, this setting should be possibly disabled.", false)]
        public const string Business_Products_TrimWhiteSpacesFromLots = nameof(Business_Products_TrimWhiteSpacesFromLots);

        [ConfigDefinition(ConfigType.Bool, "Whether Serial numbers should be cleansed of leading/trailing white spaces before comitting to a database. Please note that if Serials are being scanned, this setting should be possibly disabled.", false)]
        public const string Business_Products_TrimWhiteSpacesFromSerials = nameof(Business_Products_TrimWhiteSpacesFromSerials);
        #endregion

        #region ReasonCodes
        [ConfigDefinition(ConfigType.Bool, "Whether adjustment in reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_AdjustInManualReason = nameof(Business_ReasonCodes_AdjustInManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Adjust In' operation. Ex: 'Misc,Reason1,Reason2'", "Misc")]
        public const string Business_ReasonCodes_AdjustIn = nameof(Business_ReasonCodes_AdjustIn);

        [ConfigDefinition(ConfigType.Bool, "Whether adjustment out reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_AdjustOutManualReason = nameof(Business_ReasonCodes_AdjustOutManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Adjust Out' operation. Ex: 'Misc,Reason1,Reason2'", "Misc")]
        public const string Business_ReasonCodes_AdjustOut = nameof(Business_ReasonCodes_AdjustOut);

        [ConfigDefinition(ConfigType.Bool, "Whether Un-receive PO reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_UnReceivePoManualReason = nameof(Business_ReasonCodes_UnReceivePoManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Po unreceive' operation. Ex: 'Mistake, Damaged,'", "Misc")]
        public const string Business_ReasonCodes_UnReceivePo = nameof(Business_ReasonCodes_UnReceivePo);

        [ConfigDefinition(ConfigType.Bool, "Whether Non-PO receiving reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_NonPoReceiveManualReason = nameof(Business_ReasonCodes_NonPoReceiveManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Non-PO receiving' operation. Ex: 'NotOnPo, Extras'", "Misc")]
        public const string Business_ReasonCodes_NonPoReceive = nameof(Business_ReasonCodes_NonPoReceive);

        [ConfigDefinition(ConfigType.Bool, "Whether Non-RMA reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_NonRmaReceiveManualReason = nameof(Business_ReasonCodes_NonRmaReceiveManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Non-RMA receiving' operation. Ex: 'NotOnRma, Extras'", "Misc")]
        public const string Business_ReasonCodes_NonRmaReceive = nameof(Business_ReasonCodes_NonRmaReceive);

        [ConfigDefinition(ConfigType.Bool, "Whether Un-receive RMA reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_UnReceiveRmaManualReason = nameof(Business_ReasonCodes_UnReceiveRmaManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'RMA unreceive' operation. Ex: 'Mistake, Damaged,'", "Misc")]
        public const string Business_ReasonCodes_UnReceiveRma = nameof(Business_ReasonCodes_UnReceiveRma);

        [ConfigDefinition(ConfigType.Bool, "Whether Unpick reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_UnpickManualReason = nameof(Business_ReasonCodes_UnpickManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Unpick' operation. Ex: 'Misc,Reason1,Reason2'", "Misc")]
        public const string Business_ReasonCodes_Unpick = nameof(Business_ReasonCodes_Unpick);

        [ConfigDefinition(ConfigType.Bool, "Whether customer return reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_CustomerReturnManualReason = nameof(Business_ReasonCodes_CustomerReturnManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Customer Return Adjustment' operation. Ex: 'Misc,Reason1,Reason2'", "Misc")]
        public const string Business_ReasonCodes_CustomerReturn = nameof(Business_ReasonCodes_CustomerReturn);

        [ConfigDefinition(ConfigType.Bool, "Whether vendor return reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_VendorReturnManualReason = nameof(Business_ReasonCodes_VendorReturnManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Vendor Return Adjustment' operation. Ex: 'Misc,Reason1,Reason2'", "Misc")]
        public const string Business_ReasonCodes_VendorReturn = nameof(Business_ReasonCodes_VendorReturn);

        [ConfigDefinition(ConfigType.Bool, "Whether Repackage reason should be entered manually or selected from list", false)]
        public const string Business_ReasonCodes_RepackageManualReason = nameof(Business_ReasonCodes_RepackageManualReason);

        [ConfigDefinition(ConfigType.MultilineString, "Comma separated list of Reason codes for 'Repackage and Tote Merge' operations. Ex: 'Misc,Reason1,Reason2'", "Misc")]
        public const string Business_ReasonCodes_Repackage = nameof(Business_ReasonCodes_Repackage);
        #endregion

        #region MultiClient
        [ConfigDefinition(ConfigType.Bool, "Whether the system should allow creating products with empty Clients", false)]
        public const string Business_MultiClient_AllowEmptyClientProducts = nameof(Business_MultiClient_AllowEmptyClientProducts);

        [ConfigDefinition(ConfigType.String, "3PL Client Invoice number prefix", "INV-")]
        public const string Business_MultiClient_InvoiceNumberPrefix = nameof(Business_MultiClient_InvoiceNumberPrefix);

        [ConfigDefinition(ConfigType.String, "Email invoice subject", "Invoice @InvoiceNumber has been generated for Billing cycle @Start to @End")]
        public const string Business_MultiClient_InvoiceEmailSubject = nameof(Business_MultiClient_InvoiceEmailSubject);

        [ConfigDefinition(ConfigType.EmailBody, "Email invoice body", "Invoice has been generated for Billing cycle @Start to @End\n@InvoiceNumber\n\nThank you!")]
        public const string Business_MultiClient_InvoiceEmailBody = nameof(Business_MultiClient_InvoiceEmailBody);

        [ConfigDefinition(ConfigType.Bool, "Whether 3PL Client Invoices storage records should be generated", false)]
        public const string Business_MultiClient_GenerateStorageRecords = nameof(Business_MultiClient_GenerateStorageRecords);

        [ConfigDefinition(ConfigType.Bool, "Whether 3PL Client Invoices service records should be generated", false)]
        public const string Business_MultiClient_GenerateServiceRecords = nameof(Business_MultiClient_GenerateServiceRecords);
        #endregion

        #region License Plates
        [ConfigDefinition(ConfigType.String, "License plate prefix", "LPN")]
        public const string Business_LicensePlates_Prefix = nameof(Business_LicensePlates_Prefix);

        [ConfigDefinition(ConfigType.Bool, "Whether License Plates allow multiple products", false)]
        public const string Business_LicensePlates_AllowMultipleProduct = nameof(Business_LicensePlates_AllowMultipleProduct);

        [ConfigDefinition(ConfigType.Bool, "Whether License Plates allow multiple packsizes", false)]
        public const string Business_LicensePlates_AllowMultiplePacksizes = nameof(Business_LicensePlates_AllowMultiplePacksizes);

        [ConfigDefinition(ConfigType.Bool, "Whether License Plates allow multiple Lots", false)]
        public const string Business_LicensePlates_AllowMultipleLots = nameof(Business_LicensePlates_AllowMultipleLots);

        [ConfigDefinition(ConfigType.Bool, "Whether License Plates allow multiple Expiries", false)]
        public const string Business_LicensePlates_AllowMultipleExpiries = nameof(Business_LicensePlates_AllowMultipleExpiries);

        [ConfigDefinition(ConfigType.Bool, "Whether LPNs are allowed to be left on the floor or always require a location", false, null, false)]
        public const string Business_LicensePlates_AllowLpnOnTheFloor = nameof(Business_LicensePlates_AllowLpnOnTheFloor);

        [ConfigDefinition(ConfigType.MultiSelect, "When Pallet label is generated, what aggregator type to be used to calculate total number of pallets", nameof(PalletCountType.ByPickTicket),
            new[] { nameof(PalletCountType.ByPickTicket), nameof(PalletCountType.ByTruckLoad), nameof(PalletCountType.ByPoNumber)})]
        public const string Business_LicensePlates_PalletCountType = nameof(Business_LicensePlates_PalletCountType);
        #endregion

        #region Cycle Counting
        [ConfigDefinition(ConfigType.Bool, "Whether Cycle count changes require approval", false)]
        public const string Business_CycleCounting_RequireApproval = nameof(Business_CycleCounting_RequireApproval);

        //[ConfigDefinition(ConfigType.Int, "Acceptable threshold in % of variance that is acceptable and does not require approval. Must have 'Require Approval' enabled", 5)]
        //public const string Business_CycleCounting_AcceptableThreshold = nameof(Business_CycleCounting_AcceptableThreshold);
        #endregion

        #region Business_CalendarView
        [ConfigDefinition(ConfigType.Int, "Start of working day in hours", 8)]
        public const string Business_CalendarView_StartOfDay = nameof(Business_CalendarView_StartOfDay);

        [ConfigDefinition(ConfigType.Int, "End of working day in hours", 18)]
        public const string Business_CalendarView_EndOfDay = nameof(Business_CalendarView_EndOfDay);

        [ConfigDefinition(ConfigType.Int, "Upper calendar view cut off", 4)]
        public const string Business_CalendarView_StartOfCalendarDay = nameof(Business_CalendarView_StartOfCalendarDay);

        [ConfigDefinition(ConfigType.Int, "Lower calendar view cut off", 22)]
        public const string Business_CalendarView_EndOfCalendarDay = nameof(Business_CalendarView_EndOfCalendarDay);

        [ConfigDefinition(ConfigType.String, "Working days of week (0-Sunday, 1-Monday, 2-Tuesday, ...)", "[1,2,3,4,5]")]
        public const string Business_CalendarView_DaysOfWeek = nameof(Business_CalendarView_DaysOfWeek);

        [ConfigDefinition(ConfigType.String, "Hidden days of week (0-Sunday, 1-Monday, 2-Tuesday, ...)", "[0]")]
        public const string Business_CalendarView_HiddenDays = nameof(Business_CalendarView_HiddenDays);

        [ConfigDefinition(ConfigType.String, "View option [month agendaWeek basicWeek listWeek agendaDay basicDay listDay]", "listWeek, agendaWeek, month")]
        public const string Business_CalendarView_Views = nameof(Business_CalendarView_Views);

        [ConfigDefinition(ConfigType.String, "View that will be loaded at start", "agendaWeek")]
        public const string Business_CalendarView_DefaultView = nameof(Business_CalendarView_DefaultView);

        [ConfigDefinition(ConfigType.Int, "Approximate number of minutes it takes to receive a PO", 30)]
        public const string Business_CalendarView_PoReceivingMinutes = nameof(Business_CalendarView_PoReceivingMinutes);
        #endregion

        //Advanced
        #region Receiving
        [ConfigDefinition(ConfigType.MultilineString, "Query that return a list of putaway bins during PO receiving",
            @"--Available parameters: @WarehouseId, @ProductId, @PacksizeId, @Quantity
select top 10
	Bins.BinCode+' @ '+Zones.ZoneCode
from
	Bins
	join Zones on Zones.Id = Bins.ZoneId and Zones.WarehouseId = @WarehouseId
    join Products on Products.Id = @ProductId
	left join InventoryLocations on Bins.Id = InventoryLocations.BinId
where
	InventoryLocations.Id is null
	and Zones.ProductHandling = 'ByProduct'
	and (coalesce(Bins.AllowedCategories, Zones.AllowedCategories) is null or Products.Category in (select lower(ltrim(rtrim(value))) from string_split(coalesce(Bins.AllowedCategories, Zones.AllowedCategories), ',')))
	and (coalesce(Bins.ClientId, Zones.ClientId) is null or coalesce(Bins.ClientId, Zones.ClientId) = Products.ClientId)	
order by
	coalesce(Bins.ClientId, Zones.ClientId) desc,
	coalesce(Bins.AllowedCategories, Zones.AllowedCategories) desc,
	Zones.ZoneCode,
	Bins.BinCode")]
        public const string Advanced_Receiving_Handheld_PutawayQuery = nameof(Advanced_Receiving_Handheld_PutawayQuery);
        #endregion

        #region Returns
        [ConfigDefinition(ConfigType.MultilineString, "Query that return a list of putaway bins during RMA receiving",
            @"--Available parameters: @WarehouseId, @ProductId, @PacksizeId, @Quantity
select top 10
	Bins.BinCode+' @ '+Zones.ZoneCode
from
	Bins
	join Zones on Zones.Id = Bins.ZoneId and Zones.WarehouseId = @WarehouseId
    join Products on Products.Id = @ProductId
	left join InventoryLocations on Bins.Id = InventoryLocations.BinId
where
	InventoryLocations.Id is null
	and Zones.ProductHandling = 'ByProduct'
	and (coalesce(Bins.AllowedCategories, Zones.AllowedCategories) is null or Products.Category in (select lower(ltrim(rtrim(value))) from string_split(coalesce(Bins.AllowedCategories, Zones.AllowedCategories), ',')))
	and (coalesce(Bins.ClientId, Zones.ClientId) is null or coalesce(Bins.ClientId, Zones.ClientId) = Products.ClientId)	
order by
	coalesce(Bins.ClientId, Zones.ClientId) desc,
	coalesce(Bins.AllowedCategories, Zones.AllowedCategories) desc,
	Zones.ZoneCode,
	Bins.BinCode")]
        public const string Advanced_Returns_Handheld_PutawayQuery = nameof(Advanced_Returns_Handheld_PutawayQuery);
        #endregion

        #region DirectMove
        [ConfigDefinition(ConfigType.MultilineString, "Query that return a list of bins during Direct move",
            @"--Available parameters: @WarehouseId, @ProductId, @PacksizeId, @Quantity
select top 10
	Bins.BinCode+' @ '+Zones.ZoneCode
from
	Bins
	join Zones on Zones.Id = Bins.ZoneId and Zones.WarehouseId = @WarehouseId
    join Products on Products.Id = @ProductId
	left join InventoryLocations on Bins.Id = InventoryLocations.BinId
where
	InventoryLocations.Id is null
	and Zones.ProductHandling = 'ByProduct'
	and (coalesce(Bins.AllowedCategories, Zones.AllowedCategories) is null or Products.Category in (select lower(ltrim(rtrim(value))) from string_split(coalesce(Bins.AllowedCategories, Zones.AllowedCategories), ',')))
	and (coalesce(Bins.ClientId, Zones.ClientId) is null or coalesce(Bins.ClientId, Zones.ClientId) = Products.ClientId)	
order by
	coalesce(Bins.ClientId, Zones.ClientId) desc,
	coalesce(Bins.AllowedCategories, Zones.AllowedCategories) desc,
	Zones.ZoneCode,
	Bins.BinCode")]
        public const string Advanced_DirectMove_Handheld_RecommendedBins = nameof(Advanced_DirectMove_Handheld_RecommendedBins);
        #endregion

        #region Synchronization
        [ConfigDefinition(ConfigType.Bool, "Generate daily inventory snapshots", false)]
        public const string Advanced_Synchronization_GenerateDailyInventorySnapshots = nameof(Advanced_Synchronization_GenerateDailyInventorySnapshots);

        [ConfigDefinition(ConfigType.Bool, "Whether uploaded Pick tickets should be accounted for inventory synchronization", true)]
        public const string Advanced_Synchronization_PickTickets = nameof(Advanced_Synchronization_PickTickets);

        [ConfigDefinition(ConfigType.Bool, "Whether uploaded POs should be accounted for inventory synchronization", true)]
        public const string Advanced_Synchronization_PurchaseOrders = nameof(Advanced_Synchronization_PurchaseOrders);

        [ConfigDefinition(ConfigType.Bool, "Whether uploaded Production orders should be accounted for inventory synchronization", true)]
        public const string Advanced_Synchronization_ProductionOrders = nameof(Advanced_Synchronization_ProductionOrders);

        [ConfigDefinition(ConfigType.Bool, "Whether uploaded RMAs orders should be accounted for inventory synchronization", true)]
        public const string Advanced_Synchronization_Returns = nameof(Advanced_Synchronization_Returns);
        #endregion

        public static readonly Guid SystemUserId = new Guid("0BE2C4D6-A793-489E-9792-FDBCB320D56C");
        public static readonly Guid DefaultShippingRuleId = new Guid("C9A8416C-75A3-4DE4-A074-F8A62852E2AA");
    }

    public enum ShippingBrokerType
    {
        Default,
        EasyPost,
        Malvern
    }
}