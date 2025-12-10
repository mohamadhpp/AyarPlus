using System.Linq.Expressions;

namespace ApiTask.Application.Interfaces.Repositories.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> ToListAsync();
        Task<IEnumerable<T>> ToListAsync(IQueryable<T> query);
        Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate);
        bool IsEmpty();
        bool HasBy(Expression<Func<T, bool>> predicate);
        Task<bool> HasByAsync(Expression<Func<T, bool>> predicate);
        int Count();
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
        IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IQueryable<T> OrderByDesc<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}