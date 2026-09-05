namespace EnterpriseKnowledgeAssistant.Api.Models
{
    public class ChatResponse
    {
        public string Answer { get; set; } = string.Empty;

        public List<SourceReference> Sources { get; set; } = new();
    }

    public class SourceReference
    {
        public string FileName { get; set; } = string.Empty;

        public int PageNumber { get; set; }

        public int ChunkIndex { get; set; }
    }
}