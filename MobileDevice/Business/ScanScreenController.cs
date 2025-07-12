using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Production;
using Pro4Soft.DataTransferObjects.Dto.Receiving;
using Pro4Soft.DataTransferObjects.Dto.Returns;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;

namespace Pro4Soft.MobileDevice.Business
{
    public abstract class ScanScreenController: BaseViewController
    {
        protected ScanScreenView View => CustomView as ScanScreenView;

        public override async Task InitBase()
        {
            View.ClearMessages();
            View.ClearToolbar();
            View.SetTitle(Title);
            await base.InitBase();
        }

        //High level common lookups
        protected virtual async Task<string> ReasonCodeLookup(Func<Task> replay, string type)
        {
            return await LoopUntilGood(async () =>
            {
                var reasonCodes = await Singleton<Web>.Instance.GetInvokeAsync<List<string>>($"api/MetaDataApi/GetReasonCodes?type={type}");
                string result;
                if (reasonCodes == null)
                    result = await View.PromptString("Reason");
                else
                    result = reasonCodes.Any() ? reasonCodes.Count == 1 ? reasonCodes.Single() : await View.PromptPicker("Reason", reasonCodes) : throw new ExceptionLocalized($"No reason codes setup for [{type}]");

                await View.PushMessage($"Reason: [{result}]", replay);
                return result;
            }, replay);
        }

        protected virtual async Task<LocationLookup> LocationLookup(Func<Task> replay, string label, BinDirection direction = BinDirection.None, bool allowEmptyScan = false)
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (allowEmptyScan)
                    key = await View.PromptString(label);
                else
                {
                    if (Singleton<Context>.Instance.ForceBinLpnScan)
                        key = await View.PromptScan(label);
                    else
                        key = await View.PromptString(label);
                }
                if (string.IsNullOrWhiteSpace(key) && allowEmptyScan)
                    return null;

                var locDetails = await Singleton<Web>.Instance.GetInvokeAsync<LocationLookup>($"hh/lookup/LocationLookup?key={key}&includeTotes=true");
                locDetails.Direction = direction;
                if (locDetails.IsLpn && locDetails.Totes.Any(c => c.PickTicketState == PickTicketState.Closed || c.PickTicketState == PickTicketState.Shipped))
                    throw new ExceptionLocalized($"LPN [{locDetails.LocationCode}] already shipped/closed");
                await View.PushMessage(
                    locDetails.IsDockDoor ? $"Dock [{locDetails.LocationCode}]" :
                    locDetails.IsLpn ? $"LPN [{locDetails.LocationCode}]" :
                    locDetails.IsBin ? $"Bin [{locDetails.LocationCode}] Zone [{locDetails.ZoneCode}]" :
                    throw new ExceptionLocalized("Invalid location"), replay);

                if (locDetails.IsLpn && locDetails.LpnBinId == null && !Singleton<Context>.Instance.AllowLpnOnFloor && direction == BinDirection.In)
                {
                    var lpnBin = await LocationLookup(null, "LPN Bulk bin...");
                    if (lpnBin.IsLpn || lpnBin.ProductHandlingEnum != ProductHandlingType.ByLpn)
                        throw new ExceptionLocalized($"Invalid LPN location [{lpnBin.LocationCode}], expected Bulk bin");
                    locDetails.LpnBinId = lpnBin.BinId;
                }
                return locDetails;
            }, replay);
        }

        protected virtual async Task<LocationLookup> BinContentsLookup(Func<Task> replay, string label, bool includeContents = false)
        {
            return await LoopUntilGood(async () =>
            {
                var key = await View.PromptScan(label);
                var url = $"hh/lookup/LocationLookup?key={key}";
                if(includeContents)
                    url += "&includeContent=true";
                var locDetails = await Singleton<Web>.Instance.GetInvokeAsync<LocationLookup>(url);
                locDetails.Direction = BinDirection.None;
                await View.PushMessage(locDetails.IsLpn ? $"LPN [{locDetails.LocationCode}]" : $"Bin [{locDetails.LocationCode}] Zone [{locDetails.ZoneCode}]", replay);
                if (locDetails.IsLpn && locDetails.LpnBinId == null && !Singleton<Context>.Instance.AllowLpnOnFloor)
                {

                    var lpnBin = await LocationLookup(null, "LPN Bulk bin...");
                    if (lpnBin.IsLpn || lpnBin.ProductHandlingEnum != ProductHandlingType.ByLpn)
                        throw new ExceptionLocalized($"Invalid LPN location [{lpnBin.LocationCode}], expected Bulk bin");
                    locDetails.LpnBinId = lpnBin.BinId;
                }

                return locDetails;
            }, replay);
        }

        protected virtual async Task<StagingLocationLookup> StagingContentsLookup(Func<Task> replay, string label)
        {
            return await LoopUntilGood(async () =>
            {
                var key = await View.PromptScan(label);
                var url = $"hh/lookup/StagingLocationLookup?key={key}";
                var locDetails = await Singleton<Web>.Instance.GetInvokeAsync<StagingLocationLookup>(url);
                
                await View.PushMessage($"Staging/Bin [{locDetails.LocationCode}] Zone [{locDetails.ZoneCode}]", replay);

                return locDetails;
            }, replay);
        }

        protected virtual async Task<ProductDetails> ProductLookup(Func<Task> replay, Guid? clientId = null, bool askPacksize = true)
        {
            return await LoopUntilGood(async () =>
            {
                string skuUpcPacksize;
                if (Singleton<Context>.Instance.ForceProductScan)
                    skuUpcPacksize = await View.PromptScan("Scan product...");
                else
                    skuUpcPacksize = await View.PromptString("Scan product...");

                var url = $"hh/lookup/ProductLookup?skuUpc={skuUpcPacksize}";
                if (clientId != null)
                    url += $"&clientId={clientId}";
                var prodDetails = await Singleton<Web>.Instance.GetInvokeAsync<List<ProductDetails>>(url);
                if (prodDetails.Count > 1)
                {
                    var multRecords = prodDetails.Select(c => c.ClientName).Distinct().OrderBy(c => c).ToList();
                    if (multRecords.Count > 1)
                    {
                        var client = await View.PromptPicker("Client", multRecords);
                        prodDetails = prodDetails.Where(c => c.ClientName == client).ToList();
                    }
                }

                if (prodDetails.Count > 1)
                {
                    var multRecords = prodDetails.Select(c => c.Sku).Distinct().OrderBy(c => c).ToList();
                    if (multRecords.Count > 1)
                    {
                        var client = await View.PromptPicker("SKU", multRecords);
                        prodDetails = prodDetails.Where(c => c.Sku == client).ToList();
                    }
                }

                if (prodDetails.Count > 1)
                    throw new ExceptionLocalized($"Multiple product records found for [{skuUpcPacksize}], Please review product setup");

                var prod = prodDetails.Single();
                if (askPacksize && prod.IsPacksizeControlled && prod.PacksizeId == null)
                {
                    var packsize = await View.PromptPacksize("Packsize", prod.Packsizes);
                    prod.PacksizeId = packsize.Id;
                    prod.Barcode = packsize.Barcode;
                    prod.PacksizeName = packsize.Name;
                    prod.EachCount = packsize.EachCount;
                    prod.Height = packsize.Height;
                    prod.Width = packsize.Width;
                    prod.Length = packsize.Length;
                    prod.Weight = packsize.Weight;
                }

                var msg = Lang.Translate($"Sku: [{prod.Sku}]");
                if (!string.IsNullOrWhiteSpace(prod.Category))
                    msg += $"\n{Lang.Translate($"Category: [{prod.Category}]")}";
                if (prod.IsPacksizeControlled && prod.PacksizeId != null)
                    msg += $"\n{Lang.Translate($"Packsize: [x{prod.EachCount}]")}";
                msg += $"\n{prod.Description}";

                await View.PushThumbnailMessage(msg, prod.ImageUrl, replay, false);

                return prod;
            }, replay);
        }

        protected virtual async Task<ToteLookup> ToteLookup(Func<Task> replay, string bigText = null, string label = null, bool onlyOpen = true)
        {
            return await LoopUntilGood(async () =>
            {
                var prompt = label ?? "Scan tote...";
                if (!string.IsNullOrWhiteSpace(bigText))
                    prompt = $"Scan tote [{bigText}]...";
                var toteCode = await View.PromptScan(prompt);
                var toteLookup = await Singleton<Web>.Instance.GetInvokeAsync<ToteLookup>($"hh/lookup/ToteLookup?toteCode={toteCode}&onlyOpen={onlyOpen}");
                await View.PushMessage($@"{Lang.Translate($"{toteLookup.ToteType} [{toteLookup.Sscc18Code}]")}
{Lang.Translate($"Pick ticket [{toteLookup.PickTicketNumber}] - [{toteLookup.PickTicketState}]")}
{Lang.Translate($"Lines [{toteLookup.Lines.Count}]")}
{Lang.Translate($"Quantity [{toteLookup.Lines.Sum(c => c.PickedQuantity)}]")}", replay, false);

                return toteLookup;
            }, replay);
        }

        protected virtual async Task<List<PurchaseOrder>> PoContainerLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForcePoScan)
                        key = await View.PromptScan("Scan PO/Container...");
                    else
                        key = await View.PromptString("Scan PO/Container...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                return await Singleton<Web>.Instance.GetInvokeAsync<List<PurchaseOrder>>($"hh/lookup/PoContainerLookup?key={key}");
            }, null);
        }

        protected virtual async Task<CustomerReturn> RmaLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForceCustomerReturnScan)
                        key = await View.PromptScan("Scan RMA...");
                    else
                        key = await View.PromptString("Scan RMA...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                var rmas = await Singleton<Web>.Instance.GetInvokeAsync<List<CustomerReturn>>($"hh/lookup/CustomerReturnLookup?key={key}");
                if (rmas.Count <= 1)
                    return rmas.Single();
                
                var rmaNum = await View.PromptPicker("RMA #", rmas.Select(c => c.CustomerReturnNumber).ToList());
                return rmas.Single(c => c.CustomerReturnNumber == rmaNum);
            }, null);
        }

        protected virtual async Task<PickTicketLookup> CartonizationResultLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForcePickTicketScan)
                        key = await View.PromptScan("Scan cartonized Tote...");
                    else
                        key = await View.PromptString("Scan cartonized Tote...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                return await Singleton<Web>.Instance.GetInvokeAsync<PickTicketLookup>($"hh/lookup/CartonizationResultLookup?key={key}");
            }, null);
        }
        
        protected virtual async Task<PickTicketLookup> PickTicketLookup(Guid? pickTicketId = null)
        {
            return await LoopUntilGood(async () =>
            {
                if (pickTicketId != null)
                    return await Singleton<Web>.Instance.GetInvokeAsync<PickTicketLookup>($"hh/lookup/PickTicketLookup?pickTicketId={pickTicketId}");
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForcePickTicketScan)
                        key = await View.PromptScan("Scan Tote/Pick ticket...");
                    else
                        key = await View.PromptString("Scan Tote/Pick ticket...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;
                return await Singleton<Web>.Instance.GetInvokeAsync<PickTicketLookup>($"hh/lookup/PickTicketLookup?key={key}");
            }, null);
        }

        protected virtual async Task<CarrierShipLicensePlate> CarrierLpnLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForceBinLpnScan)
                        key = await View.PromptScan("Scan carrier LPN...");
                    else
                        key = await View.PromptString("Scan carrier LPN...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;
                return await Singleton<Web>.Instance.GetInvokeAsync<CarrierShipLicensePlate>($"hh/floor/CarrierLpnLookup?lpn={key}");
            }, null);
        }

        protected virtual async Task<TruckLoadLookup> TruckLoadLookup(Guid? id = null, bool includeTotes = false)
        {
            return await LoopUntilGood(async () =>
            {
                if(id != null)
                    return await Singleton<Web>.Instance.GetInvokeAsync<TruckLoadLookup>($"hh/lookup/TruckLoadLookup?truckLoadId={id}&includeTotes={includeTotes}");

                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForceTruckLoadBolScan)
                        key = await View.PromptScan("Scan BOL/Truck load...");
                    else
                        key = await View.PromptString("Scan BOL/Truck load...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                return await Singleton<Web>.Instance.GetInvokeAsync<TruckLoadLookup>($"hh/lookup/TruckLoadLookup?key={key}&includeTotes={includeTotes}");
            }, null);
        }

        protected virtual async Task<List<PickTicketLookup>> WaveLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForcePickTicketScan)
                        key = await View.PromptScan("Scan Tote/PickTicket/Wave...");
                    else
                        key = await View.PromptString("Scan Tote/PickTicket/Wave...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                return await Singleton<Web>.Instance.GetInvokeAsync<List<PickTicketLookup>>($"hh/lookup/WaveLookup?key={key}");
            }, WaveLookup);
        }

        protected virtual async Task<ProductionOrder> ProductionOrderLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForceProdOrderScan)
                        key = await View.PromptScan("Scan prod order...");
                    else
                        key = await View.PromptString("Scan prod order...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                return await Singleton<Web>.Instance.GetInvokeAsync<ProductionOrder>($"hh/lookup/ProductionOrderLookup?key={key}");
            }, null);
        }

        protected virtual async Task<List<ProductionOrder>> ProductionOrderBatchLookup()
        {
            return await LoopUntilGood(async () =>
            {
                string key;
                if (AssignedTask == null)
                {
                    if (Singleton<Context>.Instance.ForceProdOrderScan)
                        key = await View.PromptScan("Scan prod order...");
                    else
                        key = await View.PromptString("Scan prod order...");
                }
                else
                    key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                return await Singleton<Web>.Instance.GetInvokeAsync<List<ProductionOrder>>($"hh/lookup/ProductionOrderBatchLookup?key={key}");
            }, null);
        }

        //Low level prompts
        protected async Task<string> PromptLot(Func<Task> replay = null)
        {
            var result = await View.PromptString("Enter/Scan lot");
            await View.PushMessage($"Lot [{result}]", replay);
            return result;
        }

        protected async Task<string> PromptExpiry(Func<Task> replay = null)
        {
            return await LoopUntilGood(async () =>
            {
                var expiry = await View.PromptDate("Enter/Scan expiry");
                var result = expiry.ToString(Singleton<Context>.Instance.ExpiryDateFormat);
                await View.PushMessage($"Expiry [{result}]", replay);
                return result;
            }, replay);
        }

        protected async Task<string> PromptSerial(Func<Task> replay = null)
        {
            var result = await View.PromptScan("Scan serial...");
            await View.PushMessage($"Serial [{result}]", replay);
            return result;
        }

        protected async Task<decimal> PromptQuantity(Func<Task> replay = null, string uom = null, string label = null)
        {
            return await LoopUntilGood(async () =>
            {
                var result = await View.PromptNumeric(label ?? $"Enter quantity{(string.IsNullOrWhiteSpace(uom) ? null : $" [{uom}]")}");
                if(result == null)
                    throw new ExceptionLocalized("Invalid value, numeric field is expected");
                if (string.IsNullOrWhiteSpace(uom) && result.Value % 1 != 0)
                    throw new ExceptionLocalized("Decimal not allowed");
                await View.PushMessage($"Quantity [{result.Value}]", replay);
                return result.Value;
            }, replay);
        }

        //Loop while exception
        protected async Task LoopUntilGood(Func<Task> action, Func<Task> replay = null)
        {
            while (true)
            {
                try
                {
                    await action.Invoke();
                    break;
                }
                catch (Exception e)
                {
                    await View.PushError(e.Message, replay);
                }
            }
        }

        protected async Task<T> LoopUntilGood<T>(Func<Task<T>> action, Func<Task> replay)
        {
            while (true)
            {
                try
                {
                    return await action.Invoke();
                }
                catch (Exception e)
                {
                    await View.PushError(e.Message, replay);
                }
            }
        }
    }
}