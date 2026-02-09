using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Contacts.Commands.Models;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Core.Mapping.Contact
{
    public partial class ContactProfile
    {
        public void EditContactMapping()
        {
            CreateMap<EditContactCommand, UserContact>()
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.contactId))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.FName} {src.LName}".Trim()));
        }
    }
}
