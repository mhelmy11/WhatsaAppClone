using WhatsappClone.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhatsappClone.Infrastructure.Interfaces
{
    public interface IStatusRepository
    {
        Task<Status> GetByIdAsync(Guid statusId);
        Task<IEnumerable<Status>> GetUserStatusesAsync(string userId);
        Task<IEnumerable<Status>> GetActiveStatusesAsync(string userId);
        Task<IEnumerable<Status>> GetContactsStatusesAsync(IEnumerable<string> contactIds);
        Task<Status> CreateAsync(Status status);
        Task<bool> UpdateAsync(Status status);
        Task<bool> DeleteAsync(Guid statusId);
        Task<bool> AddViewAsync(Guid statusId, string viewerUserId);
        Task<IEnumerable<StatusView>> GetStatusViewsAsync(Guid statusId);
        Task<int> GetViewCountAsync(Guid statusId);
        Task<bool> DeactivateExpiredStatusesAsync();
    }
}
