using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsappClone.Data.MongoModels;
using WhatsappClone.Infrastructure.Bases;

namespace WhatsappClone.Infrastructure.Interfaces
{
    public interface IStoryRepository : IMongoBaseRepository<Story>
    {
        // Task<Story> GetByIdAsync(string statusId);
        // Task<IEnumerable<Story>> GetUserStatusesAsync(string userId);
        // Task<IEnumerable<Story>> GetActiveStatusesAsync(string userId);
        // Task<IEnumerable<Story>> GetContactsStatusesAsync(IEnumerable<string> contactIds);
        // Task<Story> CreateAsync(Story status);
        // Task<bool> UpdateAsync(Story status);
        // Task<bool> DeleteAsync(string statusId);
        // Task<bool> AddViewAsync(string statusId, string viewerUserId);
        // Task<IEnumerable<StoryView>> GetStatusViewsAsync(string statusId);
        // Task<int> GetViewCountAsync(string statusId);
        // Task<bool> DeactivateExpiredStatusesAsync();
    }
}
