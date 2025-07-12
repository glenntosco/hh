using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.ScanMessages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageScanMessage : BaseScanMessage
    {
        public ImageScanMessage()
        {
            InitializeComponent();
        }

        public ImageScanMessage(byte[] bytes, Func<Task> replay, StackLayout container) : this()
        {
            Replay = replay;
            Container = container;

            ImageBox.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
        }
    }
}