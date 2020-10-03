using AltiumTest.Data.Abstractions;
using AltiumTest.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AltiumTest.Data.EF.SQLServer
{
  public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
  {
    protected readonly ApplicationContext context;

    public BaseRepository(ApplicationContext context)
    {
      this.context = context;
    }

    public async Task<T> GetByIdAsync(int id)
    {
      return await context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
      return await context.Set<T>().ToListAsync();
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
      return context.Set<T>().Where(expression);
    }

    public async Task AddAsync(T entity)
    {
      await context.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
      await context.Set<T>().AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
      context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
      context.Set<T>().RemoveRange(entities);
    }
  }
}
