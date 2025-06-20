using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class UserContact
    {
        public string UserId { get; set; }
        public string ContactId { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        public string FName { get; set; }
        public string? LNAme { get; set; }

        [NotMapped]
        public string FullName { set; get; }



        public AppUser? User { get; set; }
        public AppUser? Contact { get; set; }
    }
}
