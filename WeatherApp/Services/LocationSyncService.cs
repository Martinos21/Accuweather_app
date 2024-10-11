using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    internal class LocationSyncService
    {
        private bool _isListening;
        public async Task Start()
        {
            Geolocation.LocationChanged += Geolocation_LocationChanged;
            var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));

            if(request is not null)
            {
                _isListening = await Geolocation.StartListeningForegroundAsync(request);
            }


        }

        private void Stop()
        {
            Geolocation.LocationChanged -= Geolocation_LocationChanged;
            Geolocation.StopListeningForeground();
            _isListening = false;
        }

        private void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
