using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Messages.Commands.Models;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Messages.Commands.Handler
{
    public class MessagesCommandHandler : ResponseHandler,
                                          IRequestHandler<SendGroupMessageCommand, Response<string>>
    {
        private readonly IMessagesService messagesService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IGroupService groupService;

        public MessagesCommandHandler(IMessagesService messagesService, IHttpContextAccessor httpContextAccessor, IGroupService groupService)
        {
            this.messagesService = messagesService;
            this.httpContextAccessor = httpContextAccessor;
            this.groupService = groupService;
        }
        public async Task<Response<string>> Handle(SendGroupMessageCommand request, CancellationToken cancellationToken)
        {
            var senderId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = await messagesService.SendGroupMessage(senderId!, request.GroupId, request.Attachments, request.Content ?? "");


            return Success("Sent Successfully");
        }
    }
}
