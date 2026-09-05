namespace EnterpriseKnowledgeAssistant.Api.Models;

public class StreamResponse
{
    public string Type { get; set; } = string.Empty;

    public string? Content { get; set; }

    public List<SourceReference>? Sources { get; set; }
}