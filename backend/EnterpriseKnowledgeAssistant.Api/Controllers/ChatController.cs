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
        var response = await _chatService.GetResponseAsync(request);

        return Ok(response);
    }

    [HttpPost("stream")]
    public async Task Stream(ChatRequest request)
    {
        Response.ContentType = "text/plain";

        await foreach (var chunk in _chatService.GetResponseStreamAsync(request))
        {
            await Response.WriteAsync(chunk);
            await Response.Body.FlushAsync();
        }
    }
}
