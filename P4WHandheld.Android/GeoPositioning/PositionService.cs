using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.Support.V4.Content;
using Pro4Soft.MobileDevice.GeoPositioning;

[assembly: Xamarin.Forms.Dependency(typeof(PositionService))]
namespace Pro4Soft.MobileDevice.GeoPositioning
{
    public class PositionService : IDisposable
    {
        private static PositionService _instance;

        public static PositionService Instance => _instance ??= new PositionService();

        private readonly LocationManager _locationManager;
        private readonly LocationListener _gps;
        private readonly LocationListener _network;

        public PositionService()
        {
            _locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);

            if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.AccessCoarseLocation) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.AccessFineLocation) != Permission.Granted)
                return;
            _gps = new LocationListener(_locationManager, LocationManager.GpsProvider);
            _network = new LocationListener(_locationManager, LocationManager.NetworkProvider);
        }

        public Location GetLastLocation()
        {
            if (_gps != null && _network != null)
                return _gps.TimeAdjustedAccuracy < _network.TimeAdjustedAccuracy ? _gps.LastLocation : _network.LastLocation;
            return null;
        }

        public void Dispose()
        {
            _gps?.Dispose();
            _network?.Dispose();
            _locationManager?.Dispose();
        }

        public async Task StartPositionUpdateAsync()
        {
            if (_gps != null)
                await _gps.StartPositionUpdateAsync().ConfigureAwait(false);
            
            if (_network != null)
                await _network.StartPositionUpdateAsync().ConfigureAwait(false);
            
        }

        public void StopPositionUpdate()
        {
            _gps?.StopPositionUpdate();
            _network?.StopPositionUpdate();
        }
    }
}