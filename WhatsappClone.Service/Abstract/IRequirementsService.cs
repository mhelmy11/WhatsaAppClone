using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Service.Abstract
{
    public interface IRequirementsService
    {
        public Task<bool> IsSessionRevoked(int tid);

    }
}
