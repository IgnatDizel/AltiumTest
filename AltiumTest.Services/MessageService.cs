using AltiumTest.Data.Abstractions;
using AltiumTest.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltiumTest.Services
{
  public class MessageService: IMessageService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MessageService> _logger;
    private readonly int maxLimit = 50;

    public MessageService(IUnitOfWork unitOfWork, ILogger<MessageService> logger)
    {
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime? lastMessageTime, int limit)
    {
      if (limit > maxLimit)
        limit = maxLimit;

      return (await _unitOfWork.Messages.GetListAsync(lastMessageTime, limit))
        .OrderBy(x => x.Created)
        .Select(x => new Message(x));
      
    }

    public async Task<Message> CreateAsync(NewMessage message)
    {
      Data.Entities.Message messageEntity = new Data.Entities.Message
      {
        Created = DateTime.UtcNow,
        UserName = message.UserName,
        Text = message.Text
      };

      await _unitOfWork.Messages.AddAsync(messageEntity);
      await _unitOfWork.SaveAsync();

      return new Message(messageEntity);
    }
  }
}
