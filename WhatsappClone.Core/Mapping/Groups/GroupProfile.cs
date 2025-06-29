using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Mapping.Groups
{
    public partial class GroupProfile : Profile
    {
        public GroupProfile()
        {

            CreateGroupMapping();
            EditGroupPermissionsMapping();
            GetGroupInviteInfoMapping();
        }
    }
}
