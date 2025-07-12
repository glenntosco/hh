using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using SignaturePad.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignatureView : BaseContentView
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Action<byte[]> OnSign { get; set; }

        public SignatureView()
        {
            InitializeComponent();
        }

        public override async Task OnApearing()
        {
            TitleLabel.Text = Title;
            DescriptionLbl.Text = Description;
            await base.OnApearing();
        }

        private async void Submit(object sender, EventArgs e)
        {
            if(!PadView.IsBlank)
                OnSign?.Invoke(Utils.ReadStream(await PadView.GetImageStreamAsync(SignatureImageFormat.Png)));
        }

        private void Cancel(object sender, EventArgs e)
        {
            OnSign?.Invoke(null);
        }
    }
}