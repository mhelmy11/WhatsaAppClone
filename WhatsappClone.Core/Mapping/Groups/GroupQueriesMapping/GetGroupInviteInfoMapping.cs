using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Groups.Queries.Results;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Core.Mapping.Groups
{
    public partial class GroupProfile
    {

        public void GetGroupInviteInfoMapping()
        {
            CreateMap<Group, GetGroupInviteInfoResult>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GroupPic, opt => opt.MapFrom(src => src.GroupPictureUrl));
        }


    }
}
