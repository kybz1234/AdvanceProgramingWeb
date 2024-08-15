using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFPAW.Models;

namespace TFPAW.Service
{
    public class GeocodingService
    {
        // Patrón: Inyección de Dependencias
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Patrón: Facade
        public async Task<GeoLocation> GetGeocodeAsync(string address)
        {
            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";
            var response = await _httpClient.GetStringAsync(url);
            var json = JArray.Parse(response).FirstOrDefault();

            if (json != null)
            {
                return new GeoLocation
                {
                    Latitude = (double)json["lat"],
                    Longitude = (double)json["lon"]
                };
            }

            return null;
        }
    }
}
