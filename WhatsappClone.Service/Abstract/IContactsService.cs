using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Service.Abstract
{
    public interface IContactsService
    {

        public Task<UserContact> AddContactAsync(string userId, string contactId, string FName, string LName);


        public Task<UserContact> AddContactAsync(UserContact contact);


        public Task EditContactAsync(UserContact contact);

        public Task DeleteContactAsync(string contactId, string userId);
        List<UserContact> GetContacts(string currentUserId);
        public Task<bool> IsContactAdded(string userId, string phoneNumber);
        public bool IsContactAddedByid(string userId, string contactId);

    }
}
