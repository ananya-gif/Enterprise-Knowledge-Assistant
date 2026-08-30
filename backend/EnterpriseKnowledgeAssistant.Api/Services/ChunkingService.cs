using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class ChunkingService
{
    public List<DocumentChunk> ChunkText(
        string text,
        string documentId,
        string fileName,
        int chunkSize = 500,
        int overlap = 50)
    {
        var words = text.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        var chunks = new List<DocumentChunk>();

        var start = 0;
        var chunkIndex = 0;

        while (start < words.Length)
        {
            var length = Math.Min(
                chunkSize,
                words.Length - start);

            var chunkText = string.Join(
                " ",
                words.Skip(start).Take(length));

            chunks.Add(new DocumentChunk
            {
                DocumentId = documentId,
                FileName = fileName,
                ChunkIndex = chunkIndex,
                PageNumber = 0,
                Text = chunkText
            });

            chunkIndex++;

            start += chunkSize - overlap;
        }

        return chunks;
    }
}