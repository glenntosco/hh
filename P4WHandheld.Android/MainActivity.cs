using System;
using System.Linq;
using Android.OS;
using Android.App;
using Android.Util;
using Xamarin.Forms;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Droid;
using Matcha.BackgroundService.Droid;

namespace Pro4Soft.MobileDevice
{
    [Activity(Label = "P4W Handheld", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private DataWedgeReceiver _broadcastReceiver;

        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;

            AppDomain.CurrentDomain.UnhandledException += (o, args) =>
            {
                Log.Error("Crash Report", args.ExceptionObject.ToString());
            };

            TaskScheduler.UnobservedTaskException += (o, args) =>
            {
                Log.Error("Crash Report", args.Exception.ToString());
            };

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                e.Handled = true;
                Log.Error("Crash Report", e.Exception.ToString());
            };

            //ActivityCompat.RequestPermissions(this, new[] {
            //    Manifest.Permission.AccessCoarseLocation,
            //    Manifest.Permission.AccessFineLocation,
            //    Manifest.Permission.AccessBackgroundLocation,
            //}, 1000);

            BackgroundAggregator.Init(this);

            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);
            Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            var app = new App();

            DataWedgeReceiver.CreateProfile(this);
            
            _broadcastReceiver = new DataWedgeReceiver();
            _broadcastReceiver.ScanDataReceived += (s, scanData) =>
            {
                Log.Info("P4W", $"ScanBarcode {scanData}");
                MessagingCenter.Send(app, "ScanBarcode", scanData);
            };

            LoadApplication(app);

            //Request ignore battery optimization
            //RequestBatteryOptimizationPermission();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (Xamarin.Essentials.DeviceInfo.Version.Major >= 13 && 
                (permissions.Any(p => p.Equals("android.permission.WRITE_EXTERNAL_STORAGE")) || permissions.Any(p => p.Equals("android.permission.READ_EXTERNAL_STORAGE"))))
            {
                var wIdx = Array.IndexOf(permissions, "android.permission.WRITE_EXTERNAL_STORAGE");
                var rIdx = Array.IndexOf(permissions, "android.permission.READ_EXTERNAL_STORAGE");

                if (wIdx != -1 && wIdx < permissions.Length) 
                    grantResults[wIdx] = Permission.Granted;
                if (rIdx != -1 && rIdx < permissions.Length) 
                    grantResults[rIdx] = Permission.Granted;
            }

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_broadcastReceiver == null)
                return;
            var filter = new IntentFilter(DataWedgeReceiver.IntentAction);
            filter.AddCategory(Intent.CategoryDefault);
            Log.Info("P4W", $"Subscribed to {Intent.CategoryDefault} - {DataWedgeReceiver.IntentAction}");
            Android.App.Application.Context.RegisterReceiver(_broadcastReceiver, filter);
        }

        protected override void OnPause()
        {
            if (_broadcastReceiver != null)
            {
                Android.App.Application.Context.UnregisterReceiver(_broadcastReceiver);
                Log.Info("P4W", $"Paused {Intent.CategoryDefault} - {DataWedgeReceiver.IntentAction}");
            }
            base.OnPause();
        }

        public static void RequestBatteryOptimizationPermission()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M) 
                return;

            var intent = new Intent();
            var packageName = Instance.PackageName;
            var pm = (PowerManager)Instance.GetSystemService(PowerService);

            if (pm.IsIgnoringBatteryOptimizations(packageName)) 
                return;

            intent.SetAction(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
            intent.SetData(Android.Net.Uri.Parse("package:" + packageName));
            Instance.StartActivity(intent);
        }
    }
}