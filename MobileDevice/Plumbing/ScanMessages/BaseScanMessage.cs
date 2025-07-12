using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Plumbing.ScanMessages
{
    public class BaseScanMessage : Frame
    {
        public Func<Task> _replay;
        protected StackLayout Container;

        public Func<Task> Replay
        {
            get => _replay;
            set
            {
                _replay = value;
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    BackgroundColor = Replay == null ? Color.LightGray : Color.White;
                });
            }
        }

        protected void TapAndRemove(object sender, EventArgs e)
        {
            if (Replay == null)
                return;
            var index = Container.Children.IndexOf(this);
            while (index < Container.Children.Count)
                Container.Children.RemoveAt(index);
            Replay.Invoke();
        }

        protected void Tap(object sender, EventArgs e)
        {
            Replay?.Invoke();
        }
    }
}