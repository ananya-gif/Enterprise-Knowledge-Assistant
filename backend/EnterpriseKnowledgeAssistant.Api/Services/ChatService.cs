using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class ChatService
{
    private readonly OpenAIService _openAIService;

    public ChatService(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<ChatResponse> GetResponseAsync(ChatRequest request)
    {
        var answer = await _openAIService.GetResponseAsync(request.Message);

        return new ChatResponse
        {
            Answer = answer
        };
    }

    public async IAsyncEnumerable<string> GetResponseStreamAsync(ChatRequest request)
    {
        await foreach (var chunk in _openAIService.GetResponseStreamAsync(request.Message))
        {
            yield return chunk;
        }
    }
}