using TFPAW.Models;
using System.Threading.Tasks;

namespace TFPAW.Service
{
    // Patrón: Interface
    public interface IOpenAIService
    {
        Task<string> GenerateAnswer(List<Message> conversationHistory);
    }
}
