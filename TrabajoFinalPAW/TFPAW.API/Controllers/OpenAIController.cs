using Microsoft.AspNetCore.Mvc;
using TFPAW.Models;
using TFPAW.Service;
using System.Linq;
using System.Threading.Tasks;

namespace TFPAW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        // Dependency Injection
        private readonly IOpenAIService _openAIService;

        public OpenAIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateAnswer([FromBody] ConversationRequest request)
        {
            if (request == null || request.ConversationHistory == null || !request.ConversationHistory.Any())
            {
                return BadRequest("Conversation history cannot be empty");
            }

            var answer = await _openAIService.GenerateAnswer(request.ConversationHistory);

            return Ok(new { answer });
        }
    }
}
