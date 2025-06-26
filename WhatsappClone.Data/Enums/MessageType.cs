using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Enums
{
    public enum MessageType
    {
        Unknown = 0,
        Text = 1,
        Image = 2,
        Video = 3,
        Audio = 4,
        File = 5,
        Location = 6,
        Contact = 7,
        Sticker = 8,
        Poll = 9,
        Reaction = 10,
        VoiceNote = 11,
        AddMember = 12,
        RemoveMember = 13,
        GroupCreated = 14,
        GroupPicChanged = 15,
        GroupDeleted = 16,
        GroupSettingsChanged = 17,
        GroupMemberLeft = 18,
        GroupMemberJoined = 19,
        GroupNameChanged = 20,
        GroupDescriptionChanged = 21,
        Promote = 22,
        Revoke = 23,


    }
}
