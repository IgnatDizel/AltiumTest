using System;
using System.Threading.Tasks;

namespace AltiumTest.Data.Abstractions
{
  public interface IUnitOfWork : IDisposable
  {
    IMessageRepository Messages { get; }
    Task<int> SaveAsync();
  }
}
