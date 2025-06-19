using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Service.Abstract
{
    public interface IContactsService
    {

        public Task<UserContact> AddContactAsync(string userId, string contactId, string FName, string LName);

        public Task<bool> IsContactAdded(string userId, string contactId);
    }
}
