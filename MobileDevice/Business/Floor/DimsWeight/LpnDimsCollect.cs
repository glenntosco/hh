using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.DimsWeight
{
    [ViewController("main.lpnDimsCollect")]
    public class LpnDimsCollect : ScanScreenController
    {
        public override string Title => "Tote dimensions";

        private LocationLookup _fromLpnLookupDetails;
        private decimal? _height;
        private decimal? _length;
        private decimal? _width;
        private decimal? _weight;

        protected override async Task Init()
        {
            await AskLpn();
        }

        private async Task AskLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromLpnLookupDetails = await LocationLookup(AskLpn, "Scan LPN...");
                if (!_fromLpnLookupDetails.IsLpn)
                    throw new ExceptionLocalized("Cannot scan bin, LPN expected");
            }, AskLpn);

            await AskLength();
        }

        protected async Task AskLength()
        {
            _length = await View.PromptNumeric($"Enter length [{_fromLpnLookupDetails.LengthUnitOfMeasure}]:", _fromLpnLookupDetails.Length);
            if (_length != null)
                await View.PushMessage($"Length: [{_length} {_fromLpnLookupDetails.LengthUnitOfMeasure}]");
            else
                await View.PushMessage($"Length cleared");
            await AskWidth();
        }

        protected async Task AskWidth()
        {
            _width = await View.PromptNumeric($"Enter width [{_fromLpnLookupDetails.LengthUnitOfMeasure}]:", _fromLpnLookupDetails.Width);
            if (_width != null)
                await View.PushMessage($"Width: [{_width} {_fromLpnLookupDetails.LengthUnitOfMeasure}]");
            else
                await View.PushMessage($"Width cleared");
            await AskHeight();
        }

        protected async Task AskHeight()
        {
            _height = await View.PromptNumeric($"Enter height [{_fromLpnLookupDetails.LengthUnitOfMeasure}]:", _fromLpnLookupDetails.Height);
            if (_height != null)
                await View.PushMessage($"Height: [{_height} {_fromLpnLookupDetails.LengthUnitOfMeasure}]");
            else
                await View.PushMessage($"Height cleared");
            await AskWeight();
        }

        protected async Task AskWeight()
        {
            _weight = await View.PromptNumeric($"Enter weight [{_fromLpnLookupDetails.WeightUnitOfMeasure}]:", _fromLpnLookupDetails.Weight);
            if (_weight != null)
                await View.PushMessage($"Weight: [{_weight} {_fromLpnLookupDetails.WeightUnitOfMeasure}]");
            else
                await View.PushMessage($"Weight cleared");
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                View.InactivateMessages();
                await Singleton<Web>.Instance.PostInvokeAsync("api/LicensePlateApi/CreateOrUpdate", new
                {
                    _fromLpnLookupDetails.Id,
                    Length = _length,
                    Width = _width,
                    Height = _height,
                    _fromLpnLookupDetails.LengthUnitOfMeasure,
                    Weight = _weight,
                    _fromLpnLookupDetails.WeightUnitOfMeasure
                });

                await View.PushMessage($"[{_fromLpnLookupDetails.LocationCode}] updated");

                _fromLpnLookupDetails = null;
                _length = null;
                _height = null;
                _width = null;
                _weight = null;

                await AskLpn();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
        }
    }
}