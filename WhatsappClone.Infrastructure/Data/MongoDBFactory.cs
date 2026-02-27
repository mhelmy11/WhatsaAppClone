using MongoDB.Driver;
using System;
using WhatsappClone.Data.Helpers;

namespace WhatsappClone.Infrastructure.Data;



public interface IMongoDBFactory
{
    IMongoDatabase GetDatabase(string? dbName = null);
    IMongoCollection<T> GetCollection<T>(string? dbName = null);
}

public class MongoDBFactory : IMongoDBFactory
{
    private readonly IMongoClient _mongoClient;
    private readonly string _defaultDbName;

    public MongoDBFactory(IMongoClient mongoClient, string defaultDbName = "whatsapp_db")
    {

        _mongoClient = mongoClient;
        _defaultDbName = defaultDbName;
    }
    public IMongoDatabase GetDatabase(string? dbName = null)
        => _mongoClient.GetDatabase(dbName ?? _defaultDbName);

    public IMongoCollection<T> GetCollection<T>(string? dbName = null)
    {
        var database = GetDatabase(dbName);
        return database.GetCollection<T>(MongoExtensions.GetCollectionName<T>());
    }

}
