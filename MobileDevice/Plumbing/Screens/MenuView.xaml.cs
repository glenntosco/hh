using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens.Controls;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : BaseContentView
    {
        public MenuView()
        {
            InitializeComponent();
        }

        public override async Task OnApearing()
        {
            await base.OnApearing();
            ChildrenContainer.Children.Clear();
            foreach (var menu in Singleton<Context>.Instance.MenuChildren.Where(c => c.Children.Any() || Singleton<Context>.Instance.AvailableControllers.Value.Any(c1=>c1.StateName == c.State)))
                ChildrenContainer.Children.Add(new MenuItemButton(menu));
            BindingContext = this;
        }

        public override async void OnBackButtonPressed()
        {
            if (Singleton<Context>.Instance.MenuParent != null)
            {
                Singleton<Context>.Instance.MenuChildren = Singleton<Context>.Instance.MenuParent.Parent?.Children ?? Singleton<Context>.Instance.RootMenu;
                Singleton<Context>.Instance.MenuParent = Singleton<Context>.Instance.MenuParent.Parent;
                await Main.NavigateToView<MenuView>();
            }
            else
            {
                try
                {
                    if (await Main.Self.DisplayAlert(Lang.Translate("Logout?"), Lang.Translate("Are you sure you want to logout?"), Lang.Translate("Yes"), Lang.Translate("No")))
                    {
                        await Singleton<Context>.Instance.Logout();
                        await Main.NavigateToView<LoginView>();
                    }
                }
                catch
                {
                    await Main.NavigateToView<LoginView>();
                }
            }

            base.OnBackButtonPressed();
        }
    }

    public class ViewDescriptor
    {
        public string StateName { get; set; }
        public Type ControllerType { get; set; }
        public Type ViewType { get; set; }
    }
}