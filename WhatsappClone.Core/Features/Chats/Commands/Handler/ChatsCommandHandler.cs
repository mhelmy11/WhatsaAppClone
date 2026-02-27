using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;
using MediatR;
using WhatsappClone.Core.Features.Chats.Commands.Models;

namespace WhatsappClone.Core.Features.Chats.Commands.Handler
{
    public class ChatsCommandHandler : ResponseHandler, IRequestHandler<CreateChatCommand, Response<string>>

    {
        private readonly IMapper mapper;

        public ChatsCommandHandler(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Task<Response<string>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
