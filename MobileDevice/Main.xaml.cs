using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;
using Plugin.LatestVersion;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.MobileDevice.Business;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pro4Soft.MobileDevice
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public static Main Self;

        public Main()
        {
            InitializeComponent();
            Self = this;
        }

        public static IDispatcher DispatchThread => Self.Dispatcher;

        protected override async void OnAppearing()
        {
            try
            {
                try
                {
                    var serverVersion = new Version(Utils.DeserializeFromJson<string>(new WebClient().DownloadString($"{Web.ServerHost}/version")));
                    var localVersion = typeof(BusinessWebException).Assembly.GetName().Version;

                    if (serverVersion.Major != localVersion.Major ||
                        serverVersion.Minor != localVersion.Minor)
                        if (await DisplayAlert("New version", $"There is a new version of P4W available. Would you like to update now?", "Yes", "No"))
                        {
                            await CrossLatestVersion.Current.OpenAppInStore();
                            Process.GetCurrentProcess().CloseMainWindow();
                        }
                }
                catch(Exception ex)//Supressing checking the new version
                {
                    //await DisplayAlert("New Version", $"Cannot verify the latest version, please update manually when new version becomes available", "OK");
                }
                
                if (string.IsNullOrWhiteSpace(Singleton<Context>.Instance.Tenant))
                    await NavigateToView<TenantSelectionView>();
                else
                    await NavigateToView<LoginView>();
            }
            catch (Exception ex)
            {
                ex.ToString();
                await SetError($"{Singleton<Web>.Instance.BaseUrl} cannot be reached, check your internet connection");
                Process.GetCurrentProcess().CloseMainWindow();
                return;
            }
            base.OnAppearing();
        }

        private BaseContentView _currentView;
        protected override bool OnBackButtonPressed()
        {
            _currentView?.OnBackButtonPressed();
            return true;
        }

        public static async Task NavigateToMenu(DataTransferObjects.Dto.Generic.Menu menu, Action<BaseViewController> initController = null)
        {
            var controllerDescr = Singleton<Context>.Instance.AvailableControllers.Value.SingleOrDefault(c => c.StateName == menu.State);
            if (controllerDescr == null)
                return;

            if (!(Factory.Create(controllerDescr.ControllerType) is BaseViewController controller))
                return;

            controller.MenuItem = menu;
            initController?.Invoke(controller);

            var view = Factory.Create(controllerDescr.ViewType) as BaseContentView;
            await NavigateToView(s =>
            {
                s.Controller = controller;
            }, view);
        }

        public static async Task NavigateToController<T>(Action<BaseViewController> initController = null) where T : BaseViewController
        {
            await NavigateToController(typeof(T), initController);
        }

        public static async Task NavigateToController(Type controllerType, Action<BaseViewController> initController = null)
        {
            var controllerDescr = Singleton<Context>.Instance.AvailableControllers.Value.FirstOrDefault(c => c.ControllerType == controllerType);
            if (controllerDescr == null)
                return;

            var controller = Factory.Create(controllerDescr.ControllerType) as BaseViewController;
            initController?.Invoke(controller);

            var view = Factory.Create(controllerDescr.ViewType) as BaseContentView;
            await NavigateToView(s =>
            {
                s.Controller = controller;
            }, view);
        }

        public static async Task NavigateToView<T>(Action<T> initView = null, T instance = null) where T : BaseContentView, new()
        {
            try
            {
                if (Self.Container.Children.Any())
                {
                    (Self.Container.Children.FirstOrDefault() as BaseContentView)?.OnClosing();
                    Self.Container.Children.Clear();
                }

                instance ??= Factory.Create<T>();
                Self.Container.Children.Add(instance);
                Self._currentView = instance;
                initView?.Invoke(instance);
                await Self._currentView.OnApearing();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void RestoreTo(BaseContentView viewInstance)
        {
            if (Self.Container.Children.Any())
            {
                (Self.Container.Children.FirstOrDefault() as BaseContentView)?.OnClosing();
                Self.Container.Children.Clear();
            }

            Self.Container.Children.Add(viewInstance);
            Self._currentView = viewInstance;
        }

        public static void SetLoading()
        {
            Self.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                Self.Container.Opacity = 0.3;
                Self.Container.IsEnabled = false;
                Self.Progress.IsVisible = true;
            });
        }

        public static void SetFinishedLoading()
        {
            Self.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                Self.Container.Opacity = 1;
                Self.Container.IsEnabled = true;
                Self.Progress.IsVisible = false;
            });
        }

        public static void SetStatusBar(bool isVisible)
        {
            Self.StatusBar.IsVisible = isVisible;
        }

        public static void SetLoggedUsername()
        {
            Self.UsernameTxt.Text = Singleton<Context>.Instance.Username;
        }

        public static void SetTaskVisibility()
        {
            Self.TasksIcon.IsVisible = Singleton<Context>.Instance.Tasks.Any();
        }

        public static void SetChatVisible(bool isVisible)
        {
            if (isVisible && Self._currentView is ChatView)
                return;
            Self.ChatIcon.IsVisible = isVisible;
        }

        public static bool SetGpsLocation(bool showLoc)
        {
            Self.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Self.GeoLocation.IsVisible = true;
                    Self.GeoLocation.TextColor = showLoc ? Color.DarkGreen : Color.Red;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            return showLoc;
        }

        public static async Task SetError(string message, string title = null, string closeBtnLabel = null)
        {
            await Self.DisplayAlert(title ?? "Error", message, closeBtnLabel ?? "OK");
        }
    }
}