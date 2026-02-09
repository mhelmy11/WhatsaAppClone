using System;
using MongoDB.Driver;

namespace WhatsappClone.Infrastructure.Data;



public interface IMongoDBFactory
{
    IMongoDatabase GetDatabase(string? dbName = null);
    IMongoCollection<T> GetCollection<T>(string? collectionName = null, string? dbName = null);
}

public class MongoDBFactory : IMongoDBFactory
{
    private readonly IMongoClient _mongoClient;
    private readonly string _defaultDbName;

    public MongoDBFactory(IMongoClient mongoClient, string defaultDbName = "whatsapp_clone")
    {

        _mongoClient = mongoClient;
        _defaultDbName = defaultDbName;
    }
    public IMongoDatabase GetDatabase(string? dbName = null)
        => _mongoClient.GetDatabase(dbName ?? _defaultDbName);

    public IMongoCollection<T> GetCollection<T>(string? collectionName, string? dbName = null)
    {
        collectionName ??= typeof(T).Name.ToLowerInvariant() + "s";
        var database = GetDatabase(dbName);
        return database.GetCollection<T>(collectionName);
    }

}
