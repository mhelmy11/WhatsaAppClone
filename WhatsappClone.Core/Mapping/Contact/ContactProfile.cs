using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Mapping.Contact
{
    public partial class ContactProfile : Profile
    {
        public ContactProfile()
        {
            GetContactsMapping();
            EditContactMapping();
        }
    }
}
