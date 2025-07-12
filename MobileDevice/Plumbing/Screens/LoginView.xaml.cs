using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : BaseContentView
    {
        private LoginViewVm _vm;

        public LoginView()
        {
            InitializeComponent();
            VersionLabel.Text = typeof(BusinessWebException).Assembly.GetName().Version.ToString();
            P4WarehouseLinkSpan.Text = $"P4 Warehouse © {DateTime.Now.Year}";
        }

        public override async Task OnApearing()
        {
            Main.SetStatusBar(false);

            await base.OnApearing();

            Singleton<Web>.Instance.AuthenticationToken = null;

            BindingContext = null;
            ErrorMessage.Text = null;
            ErrorMessage.IsVisible = false;
            UsernameTxt.Text = null;
            PasswordTxt.Text = null;

            UsernameTxt.Unfocus();
            PasswordTxt.Unfocus();
            
            try
            {
                Main.SetLoading();
                Singleton<Context>.Instance.Translations = await Singleton<Web>.Instance.GetInvokeAsync<Dictionary<string, string>>($"api/Lang/GetTokens", true);
                _vm = await Singleton<Web>.Instance.GetInvokeAsync<LoginViewVm>($"data/info", true);
                BindingContext = _vm;
                _vm.LogoUrl = $"{Singleton<Web>.Instance.BaseUrl}/data/logo";
                if (string.IsNullOrWhiteSpace(_vm.WelcomeMessage))
                    _vm.WelcomeMessage = Lang.Translate("Tenant login");

                _vm.Username = App.DefaultUsername;
                _vm.Password = App.DefaultPassword;
            }
            catch (Exception e)
            {
                Singleton<Context>.Instance.Tenant = null;
                await Main.SetError(e.Message);
                await Main.NavigateToView<TenantSelectionView>();
            }
            finally
            {
                Main.SetFinishedLoading();
            }
        }

        public void SetError(string errorMessage)
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                var translated = Lang.Translate(errorMessage);
                ErrorMessage.Text = translated;
                ErrorMessage.IsVisible = true;
            });
        }

        private async void Login_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_vm.Username) || string.IsNullOrWhiteSpace(_vm.Password))
            {
                SetError("Username or password are empty");
                return;
            }

            PasswordTxt.Unfocus();
            Singleton<Web>.Instance.AuthenticationToken = null;
            Singleton<Context>.Instance.Translations = null;
            ErrorMessage.IsVisible = false;
            try
            {
                await Singleton<Context>.Instance.Login(_vm.Username, _vm.Password);
                await Main.NavigateToView<MenuView>();
                Main.SetStatusBar(true);
            }
            catch (Exception exception)
            {
                SetError(exception.Message);
                await Singleton<Context>.Instance.Logout();
            }
            finally
            {
                Main.SetFinishedLoading();
            }
        }

        private void UsernameTxt_OnCompleted(object sender, EventArgs e)
        {
            PasswordTxt.Focus();
        }

        private void PasswordTxt_OnCompleted(object sender, EventArgs e)
        {
            Login_OnClicked(null, null);
        }

        private async void OnResetTenant(object sender, EventArgs e)
        {
            Singleton<Context>.Instance.Tenant = null;
            await Main.NavigateToView<TenantSelectionView>();
        }
    }

    public class LoginViewVm : INotifyPropertyChanged
    {
        private string _languageId;
        private string _logoUrl;
        private string _welcomeMessage;
        private string _username;
        private string _password;

        public string LanguageId
        {
            get => _languageId;
            set
            {
                if (value == _languageId)
                    return;
                _languageId = value;
                OnPropertyChanged();
            }
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                if (value == _welcomeMessage)
                    return;
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        public string LogoUrl
        {
            get => _logoUrl;
            set
            {
                if (value == _logoUrl) return;
                _logoUrl = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        private ICommand _tapCommand;
        public ICommand TapCommand => _tapCommand ??= new Command<string>(c => Launcher.OpenAsync(c));
        
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}