using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.ScanMessages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextWithSubtitleMessage : BaseScanMessage
    {
        public TextWithSubtitleMessage()
        {
            InitializeComponent();
        }

        public TextWithSubtitleMessage(string message, string body, string leftSubtitle, string rightSubtitle = null, Func<Task> onClick = null):this()
        {
            TitleBox.Text = message;
            LeftSubtitleBox.Text = leftSubtitle;
            RightSubtitleBox.Text = rightSubtitle;
            BodyBox.IsVisible = !string.IsNullOrWhiteSpace(body);
            BodyBox.Text = body;
            Replay = onClick;
        }
    }
}