namespace EnterpriseKnowledgeAssistant.Api.Models;

public class DocumentUploadResponse
{
    public string FileName { get; set; } = string.Empty;

    public int PageCount { get; set; }

    public int CharacterCount { get; set; }
}