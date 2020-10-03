using AltiumTest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltiumTest.Data.Abstractions
{
  public interface IMessageRepository: IBaseRepository<Message>
  {
    Task<List<Message>> GetListAsync(DateTime? lastMessageTime, int limit);
  }
}
