using EnterpriseKnowledgeAssistant.Api.Models;

namespace EnterpriseKnowledgeAssistant.Api.Services;

public class ChunkingService
{
    public List<DocumentChunk> ChunkPages(
        List<DocumentPage> pages,
        string documentId,
        string fileName,
        int chunkSize = 500,
        int overlap = 50)
    {
        var chunks = new List<DocumentChunk>();

        var chunkIndex = 0;

        foreach (var page in pages)
        {
            var words = page.Text.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            var start = 0;

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
                    PageNumber = page.PageNumber,
                    Text = chunkText
                });

                chunkIndex++;

                start += chunkSize - overlap;
            }
        }

        return chunks;
    }
}