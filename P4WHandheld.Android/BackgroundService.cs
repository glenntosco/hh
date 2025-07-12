using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Matcha.BackgroundService;
using Pro4Soft.MobileDevice;
using Pro4Soft.MobileDevice.GeoPositioning;
using Pro4Soft.MobileDevice.Plumbing.BackgroundWorkers;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundServiceStarter))]
namespace Pro4Soft.MobileDevice
{
    public class BackgroundServiceStarter : IBackgroundServiceStarter
    {
        public void Start()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(BackgroundService));
            if (!BackgroundService.IsRunning)
                Android.App.Application.Context.StartForegroundService(intent);
        }
    }

    [Service]
    public class BackgroundService : Service
    {
        public static bool IsRunning { get; set; }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            try
            {
                var service = MainActivity.Instance.GetSystemService(NotificationService) as NotificationManager;
                if (service == null)
                    return base.OnStartCommand(intent, flags, startId);

                var channel = new NotificationChannel("P4WBackground", "P4WBackground", NotificationImportance.None)
                {
                    LockscreenVisibility = NotificationVisibility.Secret
                };

                service.CreateNotificationChannel(channel);

                var notification = new Notification.Builder(MainActivity.Instance, channel.Id).Build();

                StartForeground(startId, notification);

                _ = PositionService.Instance.StartPositionUpdateAsync();

                BackgroundAggregatorService.Add(() => new GeoLocationBackgroundWorker(() =>
                {
                    var loc = PositionService.Instance.GetLastLocation();
                    if (loc == null)
                        return null;
                    return new Location
                    {
                        Accuracy = loc.Accuracy,
                        Latitude = loc.Latitude,
                        Longitude = loc.Longitude,
                    };
                }));
                BackgroundAggregatorService.StartBackgroundService();

                IsRunning = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return base.OnStartCommand(intent, flags, startId);
        }
    }
}