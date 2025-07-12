using System;
using System.Linq;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Menu = Pro4Soft.DataTransferObjects.Dto.Generic.Menu;

namespace Pro4Soft.MobileDevice.Plumbing.Screens.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItemButton : Frame
    {
        private readonly Menu _root;

        public MenuItemButton(Menu root)
        {
            _root = root;
            InitializeComponent();
            BindingContext = _root;
        }

        private async void OnMenuItemPressed(object sender, EventArgs e)
        {
            if (!_root.Children.Any())
            {
                await Main.NavigateToMenu(_root);
                return;
            }

            Singleton<Context>.Instance.MenuChildren = _root.Children;
            Singleton<Context>.Instance.MenuParent = _root;
            await Main.NavigateToView<MenuView>();
        }
    }
}