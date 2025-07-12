using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TenantSelectionView : BaseContentView
    {
        public LoginViewVm Item { get; set; }

        public TenantSelectionView()
        {
            InitializeComponent();
            Logo.Source = string.IsNullOrWhiteSpace(Singleton<Context>.Instance.Tenant) ? 
                "https://app.p4warehouse.com/UI/assets/img/pro4soft_logo_300.png" : 
                $"{Singleton<Web>.Instance.RawBaseUrl}/UI/assets/img/pro4soft_logo_300.png";
            VersionLabel.Text = typeof(BusinessWebException).Assembly.GetName().Version.ToString();
        }

        public override async Task OnApearing()
        {
            Tenant.Text = null;
            await base.OnApearing();
        }

        private async void Tenant_OnCompleted(object sender, EventArgs e)
        {
            ErrorMessage.IsVisible = false;
            var tenant = Tenant.Text?.ToLower().Trim();
            Tenant.Text = tenant;

            if (string.IsNullOrWhiteSpace(tenant))
            {
                ErrorMessage.Text = Lang.Translate("Tenant is empty");
                ErrorMessage.IsVisible = true;
                return;
            }

            try
            {
                Singleton<Context>.Instance.Tenant = tenant;
                Singleton<Web>.Instance.RawBaseUrl = Server.IsVisible ? Server.Text?.ToLower().Trim() : Singleton<Web>.Instance.RawBaseUrl;

                await Singleton<Web>.Instance.GetInvokeAsync($"data/info");
                await Main.NavigateToView<LoginView>();
            }
            catch (Exception exception)
            {
                ErrorMessage.Text = exception.Message;
                ErrorMessage.IsVisible = true;
            }
        }

        private void RewriteBaseUrl(object sender, EventArgs e)
        {
            Server.IsVisible = !Server.IsVisible;
            if(Server.IsVisible)
                Server.Text = Singleton<Web>.Instance.RawBaseUrl;
        }
    }
}