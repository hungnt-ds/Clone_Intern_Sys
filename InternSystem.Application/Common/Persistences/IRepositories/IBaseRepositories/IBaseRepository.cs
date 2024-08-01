using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        IQueryable<T> GetAllQueryable();
        Task<IQueryable<T>> GetAllIQueryableAsync();
        Task<List<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<T> GetByIdAsync(object id);
        Task<T> GetByIdAllAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);
        //Task<PaginatedList<T>> GetPagging(IQueryable<T> query, int index, int pageSize);

        Task<T?> GetWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includeProperties);

        Task<T?> GetWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default,
            params Func<IQueryable<T>, IQueryable<T>>[] includeProperties);
    }
}
