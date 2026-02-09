using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace WhatsappClone.Infrastructure.Bases;

public interface ISqlBaseRepository<T> where T : class
{

    Task DeleteRangeAsync(ICollection<T> entities);
    Task<T> GetByIdAsync(object id);
    Task SaveChangesAsync();
    IDbContextTransaction BeginTransaction();
    void Commit();
    void RollBack();
    IQueryable<T> GetTableNoTracking();
    IQueryable<T> GetTableAsTracking();
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(ICollection<T> entities);
    Task DeleteAsync(T entity);


}
