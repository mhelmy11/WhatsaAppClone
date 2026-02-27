using System;
using System.Linq.Expressions;
using MongoDB.Driver;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Infrastructure.Data;

namespace WhatsappClone.Infrastructure.Bases;

public class MongoBaseRepository<T> : IMongoBaseRepository<T> where T : MongoBaseModel
{

    protected readonly IMongoCollection<T> _collection;

    public MongoBaseRepository(IMongoDBFactory mongoDBFactory)
    {
        _collection = mongoDBFactory.GetCollection<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, null, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        var query = filter != null ? _collection.Find(filter) : _collection.Find(_ => true);
        var results = await query.ToListAsync(cancellationToken);
        return results;
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
    }
}
