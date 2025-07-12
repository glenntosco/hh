using System;
using System.Threading.Tasks;
using Android.Locations;
using Android.OS;
using Android.Runtime;

namespace Pro4Soft.MobileDevice.GeoPositioning
{
    internal class LocationListener : Java.Lang.Object, ILocationListener
    {
        private readonly LocationManager _locationManager;
        private readonly LocationProvider _locationProvider;
        private readonly string _providerName;

        public LocationListener(LocationManager locationManager, string providerName)
        {
            _locationManager = locationManager;
            _providerName = providerName;

            _locationProvider = _locationManager.GetProvider(_providerName);
            if (_locationProvider != null)
                LastLocation = _locationManager.GetLastKnownLocation(_providerName);
        }

        public Location LastLocation { get; private set; }

        public float TimeAdjustedAccuracy
        {
            get
            {
                if (LastLocation == null)
                    return float.MaxValue;

                var ageNanos = SystemClock.ElapsedRealtimeNanos() - LastLocation.ElapsedRealtimeNanos;
                var ageSeconds = ageNanos / 1000000000;

                var accuracy = LastLocation.Accuracy;

                if (ageSeconds > 0)
                    accuracy += ageSeconds / 2;

                return accuracy;
            }
        }

        private void StartPositionUpdate()
        {
            if (_locationProvider != null)
                _locationManager.RequestLocationUpdates(_providerName, 5000, 10, this, Looper.MainLooper);
        }

        protected override void Dispose(bool dispose)
        {
            if (dispose)
                StopPositionUpdate();
        }

        public new void Dispose()
        {
            StopPositionUpdate();
        }

        public void OnLocationChanged(Location location)
        {
            LastLocation = location;
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }

        public async Task StartPositionUpdateAsync()
        {
            await Task.Run(StartPositionUpdate).ConfigureAwait(false);
        }

        public void StopPositionUpdate()
        {
            _locationManager?.RemoveUpdates(this);
        }
    }
}