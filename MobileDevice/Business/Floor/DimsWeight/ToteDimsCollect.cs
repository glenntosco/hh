using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.DimsWeight
{
    [ViewController("main.toteDimsCollect")]
    public class ToteDimsCollect : ScanScreenController
    {
        public override string Title => "Tote dimensions";

        private ToteLookup _toteDetails;
        private decimal? _height;
        private decimal? _length;
        private decimal? _width;
        private decimal? _weight;

        protected override async Task Init()
        {
            await AskTote();
        }

        protected async Task AskTote()
        {
            _toteDetails = await ToteLookup(AskTote);

            await AskLength();
        }

        protected async Task AskLength()
        {
            _length = await View.PromptNumeric($"Enter length [{_toteDetails.LengthUnitOfMeasure}]:", _toteDetails.Length);
            if (_length != null)
                await View.PushMessage($"Length: [{_length} {_toteDetails.LengthUnitOfMeasure}]");
            else
                await View.PushMessage($"Length cleared");
            await AskWidth();
        }

        protected async Task AskWidth()
        {
            _width = await View.PromptNumeric($"Enter width [{_toteDetails.LengthUnitOfMeasure}]:", _toteDetails.Width);
            if (_width != null)
                await View.PushMessage($"Width: [{_width} {_toteDetails.LengthUnitOfMeasure}]");
            else
                await View.PushMessage($"Width cleared");
            await AskHeight();
        }

        protected async Task AskHeight()
        {
            _height = await View.PromptNumeric($"Enter height [{_toteDetails.LengthUnitOfMeasure}]:", _toteDetails.Height);
            if (_height != null)
                await View.PushMessage($"Height: [{_height} {_toteDetails.LengthUnitOfMeasure}]");
            else
                await View.PushMessage($"Height cleared");
            await AskWeight();
        }

        protected async Task AskWeight()
        {
            _weight = await View.PromptNumeric($"Enter weight [{_toteDetails.WeightUnitOfMeasure}]:", _toteDetails.Weight);
            if (_weight != null)
                await View.PushMessage($"Weight: [{_weight} {_toteDetails.WeightUnitOfMeasure}]");
            else
                await View.PushMessage($"Weight cleared");
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                View.InactivateMessages();
                await Singleton<Web>.Instance.PostInvokeAsync("api/ToteMasterApi/CreateOrUpdate", new
                {
                    _toteDetails.Id,
                    Length = _length,
                    Width = _width,
                    Height = _height,
                    _toteDetails.LengthUnitOfMeasure,
                    Weight = _weight,
                    _toteDetails.WeightUnitOfMeasure,
                });

                await View.PushMessage($"[{_toteDetails.Sscc18Code}] updated");

                _toteDetails = null;
                _height = null;
                _length = null;
                _width = null;
                _weight = null;

                await AskTote();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
        }
    }
}