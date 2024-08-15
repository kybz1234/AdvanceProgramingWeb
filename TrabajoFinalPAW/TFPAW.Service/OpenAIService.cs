using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using System.Linq;
using TFPAW.Models;

namespace TFPAW.Service
{
    public class OpenAIService : IOpenAIService
    {
        private readonly string _apiKey;
        private readonly string _endpoint;

        // Patrón: Dependency Injection
        public OpenAIService(string apiKey, string endpoint)
        {
            
            _apiKey = apiKey;
            _endpoint = endpoint;
        }

        // Patrón: Strategy
        public async Task<string> GenerateAnswer(List<Message> conversationHistory)
        {
            string answer = string.Empty;
            var openai = new OpenAIAPI(_apiKey);

            var messages = conversationHistory.Select(m => new OpenAI_API.Chat.ChatMessage(
                m.Role switch
                {
                    "user" => ChatMessageRole.User,
                    "assistant" => ChatMessageRole.Assistant,
                    "system" => ChatMessageRole.System,
                    _ => ChatMessageRole.User
                },
                m.Content
            )).ToList();

            var chatRequest = new ChatRequest
            {
                Messages = messages,
                MaxTokens = 400
            };

            var result = await openai.Chat.CreateChatCompletionAsync(chatRequest);
            if (result != null && result.Choices != null && result.Choices.Any())
            {
                answer = result.Choices.FirstOrDefault()?.Message.Content;
            }

            return answer;
        }
    }
}
