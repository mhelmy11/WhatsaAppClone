using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Contacts.Queries.Results;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Core.Mapping.Contact
{
    public partial class ContactProfile : Profile
    {
        public void GetContactsMapping()
        {
            CreateMap<UserContact, GetContactsQueryResult>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contact.PhoneNumber))
                .ForMember(dest => dest.PicUrl, opt => opt.MapFrom(src => src.Contact.ProfilePicUrl))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.Contact.About))
                ;
        }
    }
}

