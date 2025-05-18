using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class UserContact
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ContactId { get; set; }


        public AppUser User { get; set; }
        public AppUser Contact { get; set; }
    }
}
