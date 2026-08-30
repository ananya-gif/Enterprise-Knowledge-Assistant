using EnterpriseKnowledgeAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly DocumentIngestionService _ingestionService;

    public DocumentController(
        DocumentIngestionService ingestionService)
    {
        _ingestionService = ingestionService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please upload a file.");
        }

        if (!Path.GetExtension(file.FileName)
                .Equals(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only PDF files are supported.");
        }

        var chunks =
            await _ingestionService.ProcessDocumentAsync(file);

        return Ok(new
        {
            fileName = file.FileName,
            chunkCount = chunks.Count,
            chunks
        });
    }
}