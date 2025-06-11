using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;



namespace WhatsappClone.Core.Mapping;

public partial class ChatProfile : Profile
{
    public ChatProfile()
    {
        //Mapping configuration for Chats
        GetChatsMapping();
        GetChatByIdMapping();
        CreateChatCommandMapping();

    }

}
