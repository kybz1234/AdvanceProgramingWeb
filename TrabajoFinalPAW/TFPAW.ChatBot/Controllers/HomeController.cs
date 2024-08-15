using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TFPAW.Service;
using TFPAW.Models;
using TFPAW.ChatBot.Models;

namespace TFPAW.ChatBot.Controllers
{
    public class HomeController : Controller
    {
        // Patrón: Inyección de Dependencias
        private readonly IOpenAIService _openAIService;
        private readonly string _initialPrompt = @"Eres TravelBuddy, un asistente virtual especializado en Costa Rica. Tu tarea es ayudar a los usuarios a encontrar todo tipo de ubicaciones en el país, incluyendo pero no limitado a:

                                                1. Ciudades y pueblos
                                                2. Playas y parques nacionales
                                                3. Hoteles y alojamientos
                                                4. Restaurantes y cafeterías
                                                5. Tiendas y centros comerciales
                                                6. Atracciones turísticas
                                                7. Servicios locales (bancos, farmacias, pulperías, etc.)

                                                Cuando respondas a una consulta sobre lugares o ubicaciones:

                                                1. Proporciona siempre el nombre completo del lugar.
                                                2. Incluye la latitud y longitud exactas de cada ubicación.
                                                3. Usa el siguiente formato estricto para cada lugar: 'Nombre del lugar, latitud, longitud'
                                                4. Coloca cada lugar en una línea separada.
                                                5. Proporciona la mayor cantidad sugerencias relevantes para cada consulta.


                                                Ejemplo de respuesta:

                                                Hotel Arenal Kioro Suites & Spa, 10.4911, -84.7490
                                                La Fortuna Waterfall, 10.4433, -84.6708
                                                Arenal Volcano National Park, 10.4631, -84.7060
                                                Mistico Arenal Hanging Bridges Park, 10.4398, -84.7728
                                                Termales Los Laureles, 10.4796, -84.6528

                                                Recuerda: Tu objetivo es proporcionar información precisa y útil sobre ubicaciones en Costa Rica, siempre en el formato especificado.";
        public HomeController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {
            ViewBag.InitialPrompt = _initialPrompt;
            return View();
        }

        // Patrón: Facade
        [HttpPost]
        [Route("Home/GenerateAnswer")]
        public async Task<IActionResult> GenerateAnswer([FromBody] ConversationRequest request)
        {
            if (request == null || request.ConversationHistory == null || !request.ConversationHistory.Any())
            {
                return BadRequest("Conversation history cannot be empty");
            }

            var initialMessage = new Message
            {
                Role = "system",
                Content = _initialPrompt
            };
            request.ConversationHistory.Insert(0, initialMessage);
            var answer = await _openAIService.GenerateAnswer(request.ConversationHistory);

            // Extrae lugares y coordenadas de la respuesta
            var places = ExtractPlacesFromAnswer(answer);

            return Json(new { answer, places });
        }

        private List<object> ExtractPlacesFromAnswer(string answer)
        {
            var places = new List<object>();
            var lines = answer.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(',').Select(p => p.Trim()).ToArray();
                if (parts.Length >= 3)
                {
                    var name = parts[0];
                    if (double.TryParse(parts[1], out double latitude) &&
                        double.TryParse(parts[2], out double longitude))
                    {
                        places.Add(new { Name = name, Latitude = latitude, Longitude = longitude });
                    }
                }
            }

            return places;
        }

        // Patrón: Facade
        [HttpPost]
        [Route("Home/GetMapData")]
        public async Task<IActionResult> GetMapData([FromBody] string location)
        {
            // Verifica que la ubicación no esté vacía.
            if (string.IsNullOrWhiteSpace(location))
            {
                return BadRequest("Ubicación no proporcionada.");
            }

            var apiUrl = $"https://localhost:5001/map/{Uri.EscapeDataString(location)}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(apiUrl);
                    return Json(new { mapData = response });
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(500, $"Error al obtener datos del mapa: {ex.Message}");
                }
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
