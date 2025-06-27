using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Groups.Commands.Models;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Core.Mapping.Groups
{
    public partial class GroupProfile
    {
        public void EditGroupPermissionsMapping()
        {
            CreateMap<EditGroupPermissionsCommand, Group>();
        }

    }



}
