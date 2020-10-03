using AltiumTest.Data.Abstractions;
using AltiumTest.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltiumTest.Data.EF.SQLServer
{
  public class MessageRepository: BaseRepository<Message>, IMessageRepository
  {
    public MessageRepository(ApplicationContext context) : base(context)
    {      
    }

    public async Task<List<Message>> GetListAsync(DateTime? lastMessageTime, int limit)
    {
      IQueryable<Message> target = 
        context.Set<Message>()
        .AsNoTracking()
        .OrderByDescending(x => x.Created);

      if (lastMessageTime.HasValue)
        target = target.Where(x => x.Created < lastMessageTime);

      return await target.Take(limit).ToListAsync();
    }
  }
}
