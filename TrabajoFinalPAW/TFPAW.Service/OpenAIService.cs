using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using System.Linq;
using TFPAW.Models;


namespace TFPAW.Service;

public interface IOpenAIService
{
    Task<string> GenerateAnswer(List<Message> conversationHistory);
}

public class OpenAIService : IOpenAIService
{
    public async Task<string> GenerateAnswer(List<Message> conversationHistory)
    {
        string apikey = "sk-None-ecllbYlwPS21jzRMv1ZXT3BlbkFJ07JcCCc2tcgHlMGJ59h2";
        string answer = string.Empty;

        var openai = new OpenAIAPI(apikey);

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

        //  var messages = conversationHistory.Select(m => new OpenAI_API.Chat.ChatMessage

        //  {
        //      Role = m.Role == "user" ? ChatMessageRole.User : ChatMessageRole.Assistant,
        //      Content = m.Content
        // }).ToList();

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
