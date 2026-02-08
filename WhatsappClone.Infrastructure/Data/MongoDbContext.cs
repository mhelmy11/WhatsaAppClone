using MongoDB.Driver;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Infrastructure.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<Status> Statuses { get; }
        IMongoDatabase Database { get; }
    }

    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<Status> Statuses => _database.GetCollection<Status>("statuses");

        public IMongoDatabase Database => _database;
    }
}
