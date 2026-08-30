using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class InMemoryVectorStore
{
    private readonly List<DocumentChunk> _chunks = new();

    public void AddChunks(IEnumerable<DocumentChunk> chunks)
    {
        _chunks.AddRange(chunks);
    }

    public List<DocumentChunk> GetAllChunks()
    {
        return _chunks;
    }
}