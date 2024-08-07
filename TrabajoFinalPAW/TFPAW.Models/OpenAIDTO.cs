namespace TFPAW.Models;

public class OpenAIDTO
{
    public string Answer { get; set; }
}

public class PromptRequest
{
    public string Prompt { get; set; }
}

public class ConversationRequest
{
    public List<Message> ConversationHistory { get; set; }
}

