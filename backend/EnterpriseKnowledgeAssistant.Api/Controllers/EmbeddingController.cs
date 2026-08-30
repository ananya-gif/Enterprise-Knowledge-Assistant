using EnterpriseKnowledgeAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmbeddingController : ControllerBase
{
    private readonly EmbeddingService _embeddingService;

    public EmbeddingController(EmbeddingService embeddingService)
    {
        _embeddingService = embeddingService;
    }

    [HttpPost]
    public async Task<IActionResult> Generate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return BadRequest("Text is required.");
        }

        var embedding =
            await _embeddingService.GenerateEmbeddingAsync(text);

        return Ok(new
        {
            text,
            dimensions = embedding.Length,
            embedding
        });
    }
}