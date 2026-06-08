using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyHiep.Api.Data;

namespace MyHiep.Api.Repositories;

public class EfRepository<T>(AppDbContext db) : IRepository<T> where T : class
{
    public IQueryable<T> Query() => db.Set<T>().AsQueryable();
    public Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = db.Set<T>().AsQueryable();
        if (predicate is not null) query = query.Where(predicate);
        return query.ToListAsync();
    }
    public Task<T?> GetByIdAsync(int id) => db.Set<T>().FindAsync(id).AsTask();
    public async Task<T> AddAsync(T entity)
    {
        db.Set<T>().Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }
    public Task UpdateAsync(T entity)
    {
        db.Set<T>().Update(entity);
        return db.SaveChangesAsync();
    }
    public Task DeleteAsync(T entity)
    {
        db.Set<T>().Remove(entity);
        return db.SaveChangesAsync();
    }
    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();
}
