using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.ScanMessages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorScanMessage : BaseScanMessage
    {
        public ErrorScanMessage()
        {
            InitializeComponent();
        }

        public ErrorScanMessage(string message, Func<Task> replay, StackLayout container) :this()
        {
            TextBox.Text = message;
            Replay = replay;
            if (Replay == null)
                BackgroundColor = Color.LightGray;
            Container = container;
        }
    }
}