using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Core.Mapping.User
{
    public partial class UserProfile
    {

        public void AddUserMapping()
        {

            CreateMap<Features.Users.Commands.Models.AddUserCommand, AppUser>();
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest, opt => opt.MapFrom(src => DateTime.UtcNow))
            //    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            //    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            //    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
        }
    }
}
