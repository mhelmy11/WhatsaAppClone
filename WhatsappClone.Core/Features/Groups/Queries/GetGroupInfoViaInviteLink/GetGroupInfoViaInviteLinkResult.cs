using System ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Groups.Queries.GetGroupInfoViaInviteLink
{
    public record GetGroupInfoViaInviteLinkResult(
        string GroupPic ,
        string GroupName ,
        long CreatorUserId ,
        DateTime CreationDate ,
        //List<MutualContactDto> MutualContacts, //TODO.....
        int MembresCount,
        bool IsAlreadyMember
        );
    
}
