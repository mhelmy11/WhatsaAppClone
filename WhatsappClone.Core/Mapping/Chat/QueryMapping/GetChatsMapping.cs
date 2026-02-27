using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Chats.Queries.Results;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Core.Mapping;

public partial class ChatProfile
{
    public void GetChatsMapping()
    {
        CreateMap<Chat, GetChatsResponse>(MemberList.Destination)
        .ForMember(dest => dest.ChatImage, opt => opt.MapFrom(src => src.Receiver.ProfilePicUrl));

    }

}
