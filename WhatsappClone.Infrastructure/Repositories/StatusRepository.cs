using MongoDB.Driver;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Data.MongoModels;

namespace WhatsappClone.Infrastructure.Repositories
{
    public class StoryRepository : MongoBaseRepository<Story>, IStoryRepository
    {
        public StoryRepository(IMongoDBFactory mongoDbFactory) : base(mongoDbFactory)
        {
        }


        // private readonly IMongoCollection<Status> _statuses;
        // private readonly IMongoDatabase database;

        // public StatusRepository(IMongoDBFactory mongoDbFactory)
        // {
        //     database = mongoDbFactory.GetDatabase();
        //     _statuses = mongoDbFactory.GetCollection<Status>("Statuses");
        // }

        // public async Task<Status> GetByIdAsync(Guid statusId)
        // {
        //     return await _statuses.Find(s => s.StatusId == statusId && s.IsActive).FirstOrDefaultAsync();
        // }

        // public async Task<IEnumerable<Status>> GetUserStatusesAsync(string userId)
        // {
        //     return await _statuses.Find(s => s.UserId == userId && s.IsActive)
        //         .SortByDescending(s => s.CreatedAt)
        //         .ToListAsync();
        // }

        // public async Task<IEnumerable<Status>> GetActiveStatusesAsync(string userId)
        // {
        //     var now = DateTime.UtcNow;
        //     return await _statuses.Find(s =>
        //         s.UserId == userId &&
        //         s.IsActive &&
        //         s.ExpiresAt > now)
        //         .SortByDescending(s => s.CreatedAt)
        //         .ToListAsync();
        // }

        // public async Task<IEnumerable<Status>> GetContactsStatusesAsync(IEnumerable<string> contactIds)
        // {
        //     var now = DateTime.UtcNow;
        //     return await _statuses.Find(s =>
        //         contactIds.Contains(s.UserId) &&
        //         s.IsActive &&
        //         s.ExpiresAt > now)
        //         .SortByDescending(s => s.CreatedAt)
        //         .ToListAsync();
        // }

        // public async Task<Status> CreateAsync(Status status)
        // {
        //     await _statuses.InsertOneAsync(status);
        //     return status;
        // }

        // public async Task<bool> UpdateAsync(Status status)
        // {
        //     var result = await _statuses.ReplaceOneAsync(
        //         s => s.StatusId == status.StatusId,
        //         status);
        //     return result.ModifiedCount > 0;
        // }

        // public async Task<bool> DeleteAsync(Guid statusId)
        // {
        //     var update = Builders<Status>.Update.Set(s => s.IsActive, false);
        //     var result = await _statuses.UpdateOneAsync(
        //         s => s.StatusId == statusId,
        //         update);
        //     return result.ModifiedCount > 0;
        // }

        // public async Task<bool> AddViewAsync(Guid statusId, string viewerUserId)
        // {
        //     // Check if user already viewed
        //     var status = await _statuses.Find(s => s.StatusId == statusId).FirstOrDefaultAsync();
        //     if (status == null || status.Views.Any(v => v.UserId == viewerUserId))
        //         return false;

        //     var newView = new StatusView
        //     {
        //         UserId = viewerUserId,
        //         ViewedAt = DateTime.UtcNow
        //     };

        //     var update = Builders<Status>.Update
        //         .Push(s => s.Views, newView)
        //         .Inc(s => s.ViewCount, 1);

        //     var result = await _statuses.UpdateOneAsync(
        //         s => s.StatusId == statusId,
        //         update);

        //     return result.ModifiedCount > 0;
        // }

        // public async Task<IEnumerable<StatusView>> GetStatusViewsAsync(Guid statusId)
        // {
        //     var status = await _statuses.Find(s => s.StatusId == statusId).FirstOrDefaultAsync();
        //     return status?.Views ?? new List<StatusView>();
        // }

        // public async Task<int> GetViewCountAsync(Guid statusId)
        // {
        //     var status = await _statuses.Find(s => s.StatusId == statusId).FirstOrDefaultAsync();
        //     return status?.ViewCount ?? 0;
        // }

        // public async Task<bool> DeactivateExpiredStatusesAsync()
        // {
        //     var now = DateTime.UtcNow;
        //     var update = Builders<Status>.Update.Set(s => s.IsActive, false);
        //     var result = await _statuses.UpdateManyAsync(
        //         s => s.ExpiresAt <= now && s.IsActive,
        //         update);
        //     return result.ModifiedCount > 0;
        // }
    }
}
