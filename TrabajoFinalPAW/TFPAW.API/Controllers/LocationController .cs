using Microsoft.AspNetCore.Mvc;
using TFPAW.Service;
using TFPAW.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace TFPAW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly OpenStreetMapService _openStreetMapService;


        public LocationController(IOpenAIService openAIService, OpenStreetMapService openStreetMapService)
        {
            // Patrón: Dependency Injection
            _openAIService = openAIService;
            _openStreetMapService = openStreetMapService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchLocation([FromBody] ConversationRequest request)
        {
            if (request == null || request.ConversationHistory == null || !request.ConversationHistory.Any())
            {
                return BadRequest("Conversation history cannot be empty");
            }

            var initialMessage = new Message
            {
                Role = "system",
                Content = "Eres un asistente virtual llamado TravelBuddy que ayuda a los usuarios a encontrar ubicaciones en Costa Rica y mostrar detalles en un mapa."
            };
            request.ConversationHistory.Insert(0, initialMessage);

            var answer = await _openAIService.GenerateAnswer(request.ConversationHistory);

            // Dependency Injection y uso de servicios inyectados
            // Extrae nombres de lugares de la respuesta
            var placeNames = ExtractPlaceNamesFromAnswer(answer);
            Console.WriteLine($"Lugares extraídos: {string.Join(", ", placeNames)}");

            var markers = new List<MapMarker>();
            foreach (var placeName in placeNames)
            {
                var coordinates = await _openStreetMapService.GetCoordinatesForLocationAsync(placeName);
                Console.WriteLine($"Coordenadas para {placeName}: {coordinates}");
                if (coordinates.HasValue)
                {
                    markers.Add(new MapMarker
                    {
                        Name = placeName,
                        Latitude = coordinates.Value.Latitude,
                        Longitude = coordinates.Value.Longitude
                    });
                }
                else
                {
                    Console.WriteLine($"No se encontraron coordenadas para {placeName}");
                }
            }

            Console.WriteLine($"Marcadores a enviar: {JsonSerializer.Serialize(markers)}");
            return Ok(new { answer, places = markers });
        }

        // Método privado: Factory Method 
        private List<string> ExtractPlaceNamesFromAnswer(string answer)
        {
           
            var placeNames = new List<string>();

            
            var lines = answer.Split('\n');

            foreach (var line in lines)
            {
                
                if (line.Trim().Length > 0)
                {
                    placeNames.Add(line.Trim());
                }
            }

            return placeNames;
        }
    }
}
