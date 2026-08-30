using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class SemanticSearchService
{
    private readonly EmbeddingService _embeddingService;
    private readonly InMemoryVectorStore _vectorStore;

    public SemanticSearchService(
        EmbeddingService embeddingService,
        InMemoryVectorStore vectorStore)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
    }

    public async Task<List<SearchResult>> SearchAsync(
    string query,
    int topK = 3)
    {
        var queryEmbedding =
            await _embeddingService.GenerateEmbeddingAsync(query);

        var chunks = _vectorStore.GetAllChunks();

        var results = chunks
            .Select(chunk => new SearchResult
            {
                Chunk = chunk,
                Score = CosineSimilarity(
                    queryEmbedding,
                    chunk.Embedding)
            })
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .ToList();

        return results;
    }

    private static double CosineSimilarity(
        float[] vectorA,
        float[] vectorB)
    {
        if (vectorA.Length != vectorB.Length)
        {
            throw new ArgumentException(
                "Vectors must have the same dimensions.");
        }

        double dotProduct = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (int i = 0; i < vectorA.Length; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];

            magnitudeA += vectorA[i] * vectorA[i];
            magnitudeB += vectorB[i] * vectorB[i];
        }

        if (magnitudeA == 0 || magnitudeB == 0)
        {
            return 0;
        }

        return dotProduct /
               (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
    }
}