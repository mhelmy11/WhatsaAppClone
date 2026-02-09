using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WhatsappClone.Infrastructure.Bases;

public class SqlBaseRepository<T> : ISqlBaseRepository<T> where T : class
{

    protected readonly SqlDBContext _sqlDBContext;

    public SqlBaseRepository(SqlDBContext sqlDBContext)
    {
        _sqlDBContext = sqlDBContext;
    }

    public virtual async Task<T> GetByIdAsync(object id)
    {
        return await _sqlDBContext.Set<T>().FindAsync(id);
    }


    public IQueryable<T> GetTableNoTracking()
    {
        return _sqlDBContext.Set<T>().AsNoTracking().AsQueryable();
    }


    public virtual async Task AddRangeAsync(ICollection<T> entities)
    {
        await _sqlDBContext.Set<T>().AddRangeAsync(entities);
        await _sqlDBContext.SaveChangesAsync();

    }
    public virtual async Task<T> AddAsync(T entity)
    {
        await _sqlDBContext.Set<T>().AddAsync(entity);
        await _sqlDBContext.SaveChangesAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _sqlDBContext.Set<T>().Update(entity);
        await _sqlDBContext.SaveChangesAsync();

    }

    public virtual async Task DeleteAsync(T entity)
    {
        _sqlDBContext.Set<T>().Remove(entity);
        await _sqlDBContext.SaveChangesAsync();
    }
    public virtual async Task DeleteRangeAsync(ICollection<T> entities)
    {
        foreach (var entity in entities)
        {
            _sqlDBContext.Entry(entity).State = EntityState.Deleted;
        }
        await _sqlDBContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _sqlDBContext.SaveChangesAsync();
    }



    public IDbContextTransaction BeginTransaction()
    {


        return _sqlDBContext.Database.BeginTransaction();
    }

    public void Commit()
    {
        _sqlDBContext.Database.CommitTransaction();

    }

    public void RollBack()
    {
        _sqlDBContext.Database.RollbackTransaction();

    }

    public IQueryable<T> GetTableAsTracking()
    {
        return _sqlDBContext.Set<T>().AsQueryable();

    }

    public virtual async Task UpdateRangeAsync(ICollection<T> entities)
    {
        _sqlDBContext.Set<T>().UpdateRange(entities);
        await _sqlDBContext.SaveChangesAsync();
    }

}
