using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TFPAW.ChatBot.Models;
using TFPAW.Service;
using TFPAW.Models;

namespace TFPAW.ChatBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOpenAIService _openAIService;
        private readonly string _initialPrompt = "Eres un asistente virtual llamado TravelBuddy, diseñado para ayudar a las personas a encontrar lugares turísticos y hoteles en cualquier zona que indiquen de Costa Rica si tratan de preguntarte de otro sitio, comentaras amablemente que aun no esta disponible en esa locacionzacion. Usarás la API de OpenAI para proporcionar información detallada sobre los lugares y la API de OpenStreetMap para mostrar la ubicación exacta de estos lugares en el mapa.\n\nTu tarea es:\n1. Preguntar a los usuarios sobre la zona que les interesa.\n2. Proporcionar recomendaciones de lugares turísticos y hoteles en la zona indicada.\n3. Utilizar la API de OpenStreetMap para agregar los nodos de los lugares recomendados y mostrar su ubicación en el mapa.\n\nInicia la conversación preguntando al usuario: \"¡Hola! ¿En qué zona te gustaría encontrar lugares turísticos y hoteles? Proporcióname una ciudad o área específica.\"\n\nCuando el usuario indique una zona, responde con algo similar a: \"Aquí tienes algunas recomendaciones de lugares turísticos y hoteles en [nombre de la zona].\"\n\nFinalmente, proporciona la información detallada sobre cada lugar y utiliza la API de OpenStreetMap para agregar los nodos correspondientes.\n\nEjemplo de conversación:\nUsuario: \"Quiero encontrar lugares turísticos y hoteles en Barcelona.\"\nBot: \"¡Genial! Aquí tienes algunas recomendaciones de lugares turísticos y hoteles en Barcelona.\"\n1. Sagrada Familia (Nodo: [ID del nodo en Open Street Map])\n2. Park Güell (Nodo: [ID del nodo en Open Street Map])\n3. Hotel Arts Barcelona (Nodo: [ID del nodo en Open Street Map])\n\nPuedes preguntarme más detalles sobre cualquier lugar o pedir nuevas recomendaciones. ¡Estoy aquí para ayudarte a planificar tu viaje perfecto!\"\n\nRecuerda siempre proporcionar información precisa y actualizada, y asegurarte de que los nodos de OpenStreetMap sean correctos.";


        public HomeController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {

            ViewBag.InitialPrompt = _initialPrompt;
            return View();

        }

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

            return Json(new { answer });
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
