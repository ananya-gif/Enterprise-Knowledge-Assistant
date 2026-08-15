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
    public IActionResult Chat(ChatRequest request)
    {
        var response = _chatService.GetResponse(request);

        return Ok(response);
    }
}