using EnterpriseKnowledgeAssistant.Api.Models;
using EnterpriseKnowledgeAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat(ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message cannot be empty.");
        }

        var response = await _chatService.GetResponseAsync(request);

        return Ok(response);
    }

    [HttpPost("stream")]
    public async Task<IActionResult> Stream(ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message cannot be empty.");
        }

        Response.ContentType = "application/x-ndjson";

        await foreach (var response in
            _chatService.GetResponseStreamAsync(request))
        {
            var json =
                System.Text.Json.JsonSerializer.Serialize(response);

            await Response.WriteAsync(json);
            await Response.WriteAsync("\n");
            await Response.Body.FlushAsync();
        }

        return new EmptyResult();
    }
}
