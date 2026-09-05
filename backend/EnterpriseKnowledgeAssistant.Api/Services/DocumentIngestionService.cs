using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class DocumentIngestionService
{
    private readonly DocumentService _documentService;
    private readonly ChunkingService _chunkingService;
    private readonly EmbeddingService _embeddingService;
    private readonly InMemoryVectorStore _vectorStore;

    public DocumentIngestionService(
        DocumentService documentService,
        ChunkingService chunkingService,
        EmbeddingService embeddingService,
        InMemoryVectorStore vectorStore)
    {
        _documentService = documentService;
        _chunkingService = chunkingService;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
    }

    public async Task<List<DocumentChunk>> ProcessDocumentAsync(
        IFormFile file)
    {
        var documentId = Guid.NewGuid().ToString();

        var pages = _documentService.ExtractPages(file);

        var chunks = _chunkingService.ChunkPages(
            pages,
            documentId,
            file.FileName);

        foreach (var chunk in chunks)
        {
            chunk.Embedding =
                await _embeddingService.GenerateEmbeddingAsync(
                    chunk.Text);
        }

        _vectorStore.AddChunks(chunks);

        return chunks;
    }
}