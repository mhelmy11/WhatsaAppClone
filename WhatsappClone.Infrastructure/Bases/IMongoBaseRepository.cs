using System;
using System.Linq.Expressions;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Infrastructure.Bases;

public interface IMongoBaseRepository<T> where T : MongoBaseModel
{
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);


}
