using System.Linq.Expressions;
using InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories;
using InternSystem.Domain.Entities;
using InternSystem.Domain.Entities.BaseEntities;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Infrastructure.Persistences.Repositories.BaseRepositories;
public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
{
    private readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> Entities => _dbContext.Set<T>().Where(e => e.IsActive && !e.IsDelete);

    public IQueryable<T> GetAllQueryable()
    {
        return Entities;
    }

    public async Task<List<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        return await query.ToListAsync(cancellationToken);
    }

    public Task<IQueryable<T>> GetAllIQueryableAsync()
    {
        return Task.FromResult(Entities);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Entities.ToListAsync();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        if (entity != null && entity.IsActive && !entity.IsDelete)
        {
            return (entity);
        }
        return null;
    }
    public async Task<T> GetByIdAllAsync(object id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
            return (entity);
    }

    public async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        if (entity != null && entity.IsActive && !entity.IsDelete)
        {
            return entity;
        }
        return null;
    }

    public async Task<T> AddAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Set<T>().Attach(entity);
        }
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    //public async Task<T?> GetWithIncludesAsync(
    //    Expression<Func<T, bool>> predicate,
    //    CancellationToken cancellationToken = default,
    //    params Expression<Func<T, object>>[] includeProperties)
    //{
    //    IQueryable<T> query = _dbContext.Set<T>();

    //    query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

    //    return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    //}    

    public async Task<T?> GetWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<T?> GetWithIncludesAsync(
    Expression<Func<T, bool>> predicate,
    CancellationToken cancellationToken = default,
    params Func<IQueryable<T>, IQueryable<T>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        query = includeProperties.Aggregate(query, (current, includeProperty) => includeProperty(current));

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }
}