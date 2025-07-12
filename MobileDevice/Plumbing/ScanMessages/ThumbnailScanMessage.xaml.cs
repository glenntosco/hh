using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.ScanMessages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThumbnailScanMessage : BaseScanMessage
    {
        public ThumbnailScanMessage()
        {
            InitializeComponent();
        }

        public ThumbnailScanMessage(string message, byte[] bytes, Func<Task> replay, StackLayout container) : this()
        {
            TextBox.Text = message;
            Replay = replay;
            Container = container;
            BindingContext = this;
            
            var resource = ImageSource.FromStream(() => new MemoryStream(bytes));

            ImageBox.Source = resource;
        }
    }
}