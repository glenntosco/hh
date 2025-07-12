using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business
{
    [ViewController("main.customAction")]
    public class CustomActions : ScanScreenController
    {
        public override string Title => MenuItem?.Label??"Custom action";

        private CustomAction _action;
        private ReferenceLookupDetails _reference;
        private ProductDetails _prodDetails;
        private decimal _quantity;

        protected override async Task Init()
        {
            try
            {
                if (!MenuItem.StateParams.TryGetValue("Id", out var customActionId))
                    throw new ExceptionLocalized("Invalid customActionId");
                _action = await Singleton<Web>.Instance.GetInvokeAsync<CustomAction>($"hh/lookup/CustomActionLookup?customActionId={customActionId}");
                await AskReference();
            }
            catch (Exception e)
            {
                await View.PushError(e.Message, Init);
            }
        }

        protected async Task AskReference()
        {
            try
            {
                if (_action.ReferenceType == "Product")
                {
                    _prodDetails = await ProductLookup(AskReference);
                    _reference = new ReferenceLookupDetails
                    {
                        Id = _prodDetails.Id,
                        Client = _prodDetails.ClientName,
                        ClientId = _prodDetails.ClientId,
                        ReferenceCode = _prodDetails.Sku
                    };
                }
                else
                {
                    var refCode = await View.PromptString("Reference");
                    var refDetails = await Singleton<Web>.Instance.GetInvokeAsync<List<ReferenceLookupDetails>>($"hh/lookup/CustomActionReferenceLookup?customActionId={_action.Id}&reference={refCode}");
                    if (refDetails.Count > 1)
                    {
                        var client = await View.PromptPicker("Client", refDetails.Select(c => c.Client).OrderBy(c => c).ToList());
                        _reference = refDetails.Single(c => c.Client == client);
                    }
                    else
                        _reference = refDetails.Single();

                    await View.PushMessage(_reference.Details, AskReference, false);
                }
                
                if (_action.QtyRequired)
                    await AskQuantity();
                else
                    await Process();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, AskReference);
                await AskReference();
            }
        }

        protected async Task AskQuantity()
        {
            _quantity = await PromptQuantity(AskQuantity, _prodDetails?.UnitOfMeasure?.ToString());
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/CustomAction?customActionId={_action.Id}&referenceId={_reference.Id}";
                if (_action.QtyRequired)
                    url += $"&quantity={_quantity}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                await View.PushMessage("Done!");
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                await AskReference();
            }
        }
    }
}