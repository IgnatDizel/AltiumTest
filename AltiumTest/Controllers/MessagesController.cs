using AltiumTest.Models;
using AltiumTest.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltiumTest.Controllers
{
  [ApiController]
  public class MessagesController : Controller
  {
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
      _messageService = messageService;
    }

    [Route("api/v1/messages")]
    public async Task<IEnumerable<Message>> GetListAsync([FromQuery] DateTime? lastMessageTime, [FromQuery] int limit = 30)
    {
      IEnumerable<Services.Models.Message> messages = 
        await _messageService.GetMessagesAsync(lastMessageTime, limit);

      return messages.Select(x => new Message(x));
    }
  }
}
