using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class MessageStatusesService : IMessageStatusesService
    {
        private readonly IMessageReadStatus messageStatusRepo;

        public MessageStatusesService(IMessageReadStatus messageStatusRepo)
        {
            this.messageStatusRepo = messageStatusRepo;
        }
        public Lookup<Guid, IEnumerable<MessageReadStatus>> GetMessageStatusesDict()
        {
            var statuses = messageStatusRepo.GetTableNoTracking().ToLookup(x => x.MessageId);
            return (Lookup<Guid, IEnumerable<MessageReadStatus>>)statuses;
        }
    }
}
