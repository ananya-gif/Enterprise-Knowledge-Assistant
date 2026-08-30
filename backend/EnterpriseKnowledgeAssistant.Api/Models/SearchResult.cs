namespace EnterpriseKnowledgeAssistant.Api.Models;

public class SearchResult
{
    public DocumentChunk Chunk { get; set; } = null!;

    public double Score { get; set; }
}