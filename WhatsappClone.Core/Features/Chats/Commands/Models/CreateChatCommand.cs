using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
namespace WhatsappClone.Core.Features.Chats.Commands.Models;

public class CreateChatCommand : IRequest<Response<string>>
{

    public string Name { get; set; }
    public string Phone { get; set; }

    //CreateDTO


}
