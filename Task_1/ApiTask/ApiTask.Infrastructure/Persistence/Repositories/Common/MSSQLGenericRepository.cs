using Microsoft.EntityFrameworkCore;
using ApiTask.Application.Interfaces.Repositories.Common;
using System.Linq.Expressions;

namespace ApiTask.Infrastructure.Persistence.Repositories.Common
{
    public class MSSQLGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public MSSQLGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public bool IsEmpty()
        {
            return !_dbSet.Any();
        }

        public async Task<IEnumerable<T>> ToListAsync(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> ToListAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            // Ensure only this entity is marked as Added
            var entry = await _dbSet.AddAsync(entity);
            entry.State = EntityState.Added;

            // Save only the changes for this specific entity
            var addedEntries = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity == entity)
                .ToList();

            if (addedEntries.Any())
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            // Clear tracking to avoid conflicts
            var existingEntry = _context.ChangeTracker.Entries<T>()
                .FirstOrDefault(e => e.Entity == entity);

            if (existingEntry != null)
            {
                existingEntry.State = EntityState.Detached;
            }

            _dbSet.Update(entity);

            // Ensure only this entity is marked as Modified
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);

                // Ensure only this entity is marked as Deleted
                var entry = _context.Entry(entity);
                entry.State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().LastOrDefaultAsync(predicate);
        }

        public bool HasBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<bool> HasByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).CountAsync();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _dbSet.AsNoTracking().OrderBy(keySelector);
        }

        public IQueryable<T> OrderByDesc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _dbSet.AsNoTracking().OrderByDescending(keySelector);
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}