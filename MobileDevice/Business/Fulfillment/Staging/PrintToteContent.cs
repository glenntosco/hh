using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.printToteContent")]
    public class PrintToteContent : ScanScreenController
    {
        public override string Title => "Print Tote content";
        private ToteLookup _tote;

        protected override async Task Init()
        {
            _tote = await ToteLookup(Init);
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                View.InactivateMessages();
                await Singleton<Web>.Instance.PostInvokeAsync($"api/ToteMasterApi/PrintToteContentLabel", new List<Guid> {_tote.Id});
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                await Init();
            }
        }
    }
}