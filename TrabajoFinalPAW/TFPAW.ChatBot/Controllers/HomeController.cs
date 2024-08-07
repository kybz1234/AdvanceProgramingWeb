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
        private readonly string _initialPrompt = "Eres un asistente virtual llamado TravelBuddy, dise�ado para ayudar a las personas a encontrar lugares tur�sticos y hoteles en cualquier zona que indiquen de Costa Rica si tratan de preguntarte de otro sitio, comentaras amablemente que aun no esta disponible en esa locacionzacion. Usar�s la API de OpenAI para proporcionar informaci�n detallada sobre los lugares y la API de OpenStreetMap para mostrar la ubicaci�n exacta de estos lugares en el mapa.\n\nTu tarea es:\n1. Preguntar a los usuarios sobre la zona que les interesa.\n2. Proporcionar recomendaciones de lugares tur�sticos y hoteles en la zona indicada.\n3. Utilizar la API de OpenStreetMap para agregar los nodos de los lugares recomendados y mostrar su ubicaci�n en el mapa.\n\nInicia la conversaci�n preguntando al usuario: \"�Hola! �En qu� zona te gustar�a encontrar lugares tur�sticos y hoteles? Proporci�name una ciudad o �rea espec�fica.\"\n\nCuando el usuario indique una zona, responde con algo similar a: \"Aqu� tienes algunas recomendaciones de lugares tur�sticos y hoteles en [nombre de la zona].\"\n\nFinalmente, proporciona la informaci�n detallada sobre cada lugar y utiliza la API de OpenStreetMap para agregar los nodos correspondientes.\n\nEjemplo de conversaci�n:\nUsuario: \"Quiero encontrar lugares tur�sticos y hoteles en Barcelona.\"\nBot: \"�Genial! Aqu� tienes algunas recomendaciones de lugares tur�sticos y hoteles en Barcelona.\"\n1. Sagrada Familia (Nodo: [ID del nodo en Open Street Map])\n2. Park G�ell (Nodo: [ID del nodo en Open Street Map])\n3. Hotel Arts Barcelona (Nodo: [ID del nodo en Open Street Map])\n\nPuedes preguntarme m�s detalles sobre cualquier lugar o pedir nuevas recomendaciones. �Estoy aqu� para ayudarte a planificar tu viaje perfecto!\"\n\nRecuerda siempre proporcionar informaci�n precisa y actualizada, y asegurarte de que los nodos de OpenStreetMap sean correctos.";


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
