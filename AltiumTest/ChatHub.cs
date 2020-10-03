using AltiumTest.Data.Abstractions;
using AltiumTest.Data.Entities;
using AltiumTest.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AltiumTest
{
  public class ChatHub : Hub
  {
    private readonly IMessageService _messageService;
    private static readonly ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();

    public ChatHub(IMessageService messageService) : base()
    {
      _messageService = messageService;
    }

    public override async Task OnConnectedAsync()
    {
      if (Context.GetHttpContext().Request.Query.TryGetValue("userName", out StringValues userName))
      {
        await base.OnConnectedAsync();

        if (users.TryAdd(Context.ConnectionId, userName.ToString()))
          await this.Clients.All.SendAsync("UpdateCount", users.Select(x => x.Value).Distinct().Count());        
      }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      await base.OnDisconnectedAsync(exception);

      if (users.TryRemove(Context.ConnectionId, out string _))
        await this.Clients.All.SendAsync("UpdateCount", users.Select(x => x.Value).Distinct().Count());
    }

    public async Task Send(Models.NewMessage newMessage)
    {
      Services.Models.NewMessage message =
        new Services.Models.NewMessage(newMessage.UserName, newMessage.Text);
     
      await _messageService.CreateAsync(message);
      await this.Clients.All.SendAsync("Send", message);
    }
  }
}
