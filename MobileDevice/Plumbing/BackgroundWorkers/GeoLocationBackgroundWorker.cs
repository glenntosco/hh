using System;
using System.Threading.Tasks;
using Matcha.BackgroundService;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Essentials;

namespace Pro4Soft.MobileDevice.Plumbing.BackgroundWorkers
{
    public interface IBackgroundServiceStarter
    {
        void Start();
    }

    public class GeoLocationBackgroundWorker : IPeriodicTask
    {
        private readonly Func<Location> _getPosition;

        public GeoLocationBackgroundWorker(Func<Location> getPosition)
        {
            _getPosition = getPosition;
            Interval = TimeSpan.FromMinutes(2);
            //Interval = TimeSpan.FromSeconds(15);
        }

        public TimeSpan Interval { get; set; }

        public async Task<bool> StartJob()
        {
            try
            {
                if (Singleton<Context>.Instance.TrackGeoLocation != true)
                    return true;

                var loc = _getPosition();
                if (!Main.SetGpsLocation(loc != null))
                    return true;
                if (loc != null)
                    await Singleton<Web>.Instance.GetInvokeAsync($"api/UserApi/UpdateGeoLocation?lon={loc.Longitude}&lat={loc.Latitude}", true);

                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return false;
            }
            catch (FeatureNotEnabledException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return false;
            }
            catch (PermissionException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return true;
            }
        }
    }
}