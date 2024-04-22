using System.Linq.Expressions;
using MovieCatalog.Entities;

namespace MovieCatalog.Repository.IRepository
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, 
                IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, 
            bool isTracking = true);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, 
            bool isTracking = true);

        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}
