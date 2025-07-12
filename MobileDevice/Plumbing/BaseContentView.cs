using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Pro4Soft.MobileDevice.Business;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Plumbing
{
    public class BaseContentView : ContentView, INotifyPropertyChanged
    {
        public virtual void OnBackButtonPressed()
        {
            
        }

        private object _prevContext;
        public virtual void OnClosing()
        {
            _prevContext = BindingContext;
            BindingContext = null;
        }

        public virtual async Task OnApearing()
        {
            BindingContext = _prevContext;
            if(Controller != null)
                await Controller.InitBase();
        }

        private BaseViewController _controller;
        public BaseViewController Controller
        {
            get => _controller;
            set
            {
                if (_controller != null)
                    _controller.CustomView = null;
                _controller = value;
                if (_controller != null)
                    _controller.CustomView = this;
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        [MobileDevice.NotifyPropertyChangedInvocator]
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
