using AltiumTest.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltiumTest.Services
{
  public interface IMessageService
  {
    Task<IEnumerable<Message>> GetMessagesAsync(DateTime? lastMessageTime, int limit);
    Task<Message> CreateAsync(NewMessage message);
  }
}
