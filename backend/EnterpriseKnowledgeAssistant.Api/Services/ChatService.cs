using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class ChatService
{
    private readonly OpenAIService _openAIService;
    private readonly RagService _ragService;

    public ChatService(
        OpenAIService openAIService,
        RagService ragService)
    {
        _openAIService = openAIService;
        _ragService = ragService;
    }

    public async Task<ChatResponse> GetResponseAsync(ChatRequest request)
    {
        var results = await _ragService.SearchAsync(request.Message);

        var answer = await _ragService.GenerateAnswerAsync(
            request.Message,
            results);

        var sources = results
            .Select(result => new SourceReference
            {
                FileName = result.Chunk.FileName,
                PageNumber = result.Chunk.PageNumber,
                ChunkIndex = result.Chunk.ChunkIndex
            })
            .ToList();

        return new ChatResponse
        {
            Answer = answer,
            Sources = sources
        };
    }

    public async IAsyncEnumerable<string> GetResponseStreamAsync(
        ChatRequest request)
    {
        var context = await _ragService.BuildContextAsync(
            request.Message);

        await foreach (var chunk in
            _openAIService.GetResponseStreamAsync(context))
        {
            yield return chunk;
        }
    }
}