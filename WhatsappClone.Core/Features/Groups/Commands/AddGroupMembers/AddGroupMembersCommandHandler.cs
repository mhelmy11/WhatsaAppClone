using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;

namespace WhatsappClone.Core.Features.Groups.Commands.AddGroupMembers
{
    public class AddGroupMembersCommandHandler : ResponseHandler, IRequestHandler<AddGroupMembersCommand, Response<AddGroupMembersResult>>
    {
        private readonly SqlDBContext dBContext;
        private readonly IMongoDBFactory mongoDBFactory;

        public AddGroupMembersCommandHandler(SqlDBContext dBContext , IMongoDBFactory mongoDBFactory)
        {
            this.dBContext = dBContext;
            this.mongoDBFactory = mongoDBFactory;
        }
        public async Task<Response<AddGroupMembersResult>> Handle(AddGroupMembersCommand request, CancellationToken cancellationToken)
        {

            bool isGroupExisiting = dBContext.Groups.Any(g => g.Id == request.GroupId);
            if (!isGroupExisiting)
            {
                return BadRequest<AddGroupMembersResult>("this group is not exisiting");
            }

            var addedMembers = request.Members.Select( s => new GroupMember {
                GroupId = request.GroupId,
                Role = MemberRole.Member,
                UserId = s,
                JoinedAt = DateTime.UtcNow,        
            }).ToList();

            await dBContext.AddRangeAsync(addedMembers);

            dBContext.SaveChanges();

            return Success<AddGroupMembersResult>(null , "Members Added Successfully");
            
        }
    }
}
