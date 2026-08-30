using EnterpriseKnowledgeAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly SemanticSearchService _searchService;

    public SearchController(SemanticSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest("Query is required.");
        }

        var results = await _searchService.SearchAsync(
            request.Query,
            request.TopK);

        return Ok(results);
    }
}

public class SearchRequest
{
    public string Query { get; set; } = string.Empty;

    public int TopK { get; set; } = 3;
}