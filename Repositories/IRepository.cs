using System.Linq.Expressions;

namespace MyHiep.Api.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null);
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
}
