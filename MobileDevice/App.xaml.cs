using System;
using Matcha.BackgroundService;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.BackgroundWorkers;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice
{
    public partial class App : Application
    {

#if DEBUG
        //public const string BaseUrl = "http://www.rodionpronin.com:2020";
        public const string DefaultUsername = "hh";
        public const string DefaultPassword = "hh";
#else
        public const string BaseUrl = "https://app.p4warehouse.com";
        public const string DefaultUsername = null;
        public const string DefaultPassword = null;
#endif

        public bool IsInForeground { get; private set; } = true;

        public static App CurrentApp => (App)Current;

        public App()
        {
            InitializeComponent();
            MainPage = new Main();
        }

        protected override void OnStart()
        {
            IsInForeground = true;
            DependencyService.Get<IBackgroundServiceStarter>().Start();
        }

        protected override void OnSleep()
        {
            IsInForeground = false;
            //BackgroundAggregatorService.StopBackgroundService();
        }

        protected override void OnResume()
        {
            IsInForeground = true;
            //BackgroundAggregatorService.StartBackgroundService();
        }
    }
}
