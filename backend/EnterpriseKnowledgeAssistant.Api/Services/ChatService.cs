using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class ChatService
{
    public ChatResponse GetResponse(ChatRequest request)
    {
        return new ChatResponse
        {
            Answer = $"You asked: {request.Message}"
        };
    }
}