using EnterpriseKnowledgeAssistant.Api.Models;
using EnterpriseKnowledgeAssistant.Api.Services;

namespace EnterpriseKnowledgeAssistant.Tests;

public class ChunkingServiceTests
{
    [Fact]
    public void ChunkPages_ShouldCreateMultipleChunks_WhenTextExceedsChunkSize()
    {
        // Arrange
        var service = new ChunkingService();

        var words = Enumerable
            .Range(1, 120)
            .Select(i => $"word{i}");

        var page = new DocumentPage
        {
            PageNumber = 2,
            Text = string.Join(" ", words)
        };

        // Act
        var chunks = service.ChunkPages(
            new List<DocumentPage> { page },
            "document-1",
            "policy.pdf",
            chunkSize: 50,
            overlap: 10);

        // Assert
        Assert.True(chunks.Count > 1);
        Assert.All(chunks, chunk =>
        {
            Assert.Equal(2, chunk.PageNumber);
            Assert.Equal("policy.pdf", chunk.FileName);
            Assert.Equal("document-1", chunk.DocumentId);
        });
    }

    [Fact]
    public void ChunkPages_ShouldApplyOverlapBetweenChunks()
    {
        // Arrange
        var service = new ChunkingService();

        var page = new DocumentPage
        {
            PageNumber = 1,
            Text = string.Join(
                " ",
                Enumerable.Range(1, 60).Select(i => $"word{i}"))
        };

        // Act
        var chunks = service.ChunkPages(
            new List<DocumentPage> { page },
            "document-1",
            "policy.pdf",
            chunkSize: 50,
            overlap: 10);

        // Assert
        Assert.True(chunks.Count >= 2);

        var firstWords = chunks[0].Text.Split(' ');
        var secondWords = chunks[1].Text.Split(' ');

        Assert.Equal(
            firstWords[^10..],
            secondWords[..10]);
    }

    [Fact]
    public void ChunkPages_ShouldPreservePageNumbers()
    {
        // Arrange
        var service = new ChunkingService();

        var pages = new List<DocumentPage>
        {
            new()
            {
                PageNumber = 1,
                Text = "Page one content"
            },
            new()
            {
                PageNumber = 2,
                Text = "Page two content"
            }
        };

        // Act
        var chunks = service.ChunkPages(
            pages,
            "document-1",
            "policy.pdf");

        // Assert
        Assert.Equal(2, chunks.Count);
        Assert.Equal(1, chunks[0].PageNumber);
        Assert.Equal(2, chunks[1].PageNumber);
    }
}