using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class InMemoryVectorStore
{
    private readonly List<DocumentChunk> _chunks = new();

    public void AddChunks(IEnumerable<DocumentChunk> chunks)
    {
        var chunkList = chunks.ToList();

        if (chunkList.Count == 0)
        {
            return;
        }

        var documentId = chunkList[0].DocumentId;

        _chunks.RemoveAll(chunk =>
            chunk.DocumentId == documentId);

        _chunks.AddRange(chunkList);
    }

    public List<DocumentChunk> GetAllChunks()
    {
        return _chunks;
    }
}