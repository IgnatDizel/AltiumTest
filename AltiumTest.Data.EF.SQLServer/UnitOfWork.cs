using AltiumTest.Data.Abstractions;
using System.Threading.Tasks;

namespace AltiumTest.Data.EF.SQLServer
{
  public sealed class UnitOfWork: IUnitOfWork
  {
    private readonly ApplicationContext _context;

    public UnitOfWork(ApplicationContext context)
    {
      _context = context;
      Messages = new MessageRepository(_context);
    }

    public IMessageRepository Messages { get; private set; }

    public async Task<int> SaveAsync()
    {
      return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
      _context.Dispose();
    }
  }
}
