using OpenAI.Embeddings;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class EmbeddingService
{
    private readonly EmbeddingClient _client;

    public EmbeddingService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException(
                "OpenAI API key is not configured.");

        _client = new EmbeddingClient(
            "text-embedding-3-small",
            apiKey);
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var response = await _client.GenerateEmbeddingAsync(text);

        return response.Value.ToFloats().ToArray();
    }
}