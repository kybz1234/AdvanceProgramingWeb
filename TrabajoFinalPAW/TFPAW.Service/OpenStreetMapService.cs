using TFPAW.Architecture;
using System.Net.Http.Json;
using System.Text.Json;

namespace TFPAW.Service
{
    public class OpenStreetMapService
    {
        private readonly IRestProvider _restProvider;
        private const string BaseUrl = "https://nominatim.openstreetmap.org";

        public OpenStreetMapService(IRestProvider restProvider)
        {
            // Patrón: Dependency Injection
            _restProvider = restProvider;
        }

        // Patrón: Facade
        public async Task<string> GetMapDataByBoundingBoxAsync(double minLon, double minLat, double maxLon, double maxLat)
        {
            var bbox = $"{minLon},{minLat},{maxLon},{maxLat}";
            var endpoint = $"{BaseUrl}/api/0.6/map?bbox={bbox}";
            return await _restProvider.GetAsync(endpoint, null);
        }

        // Patrón: Facade
        public async Task<string> GetNodeDataAsync(long nodeId)
        {
            var endpoint = $"{BaseUrl}/api/0.6/node/{nodeId}";
            return await _restProvider.GetAsync(endpoint, null);
        }

        // Patrón: Facade
        public async Task<(double Latitude, double Longitude)?> GetCoordinatesForLocationAsync(string location)
        {
            var endpoint = $"{BaseUrl}/search?q={Uri.EscapeDataString(location)}&format=json&limit=1";
            var response = await _restProvider.GetAsync(endpoint, null);

            var results = JsonSerializer.Deserialize<List<OpenStreetMapResult>>(response);
            var firstResult = results?.FirstOrDefault();
            if (firstResult != null)
            {
                return (firstResult.Lat, firstResult.Lon);
            }

            return null;
        }
    }

    public class OpenStreetMapResult
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
