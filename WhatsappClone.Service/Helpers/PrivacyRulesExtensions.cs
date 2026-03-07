using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Service.Helpers
{
    namespace WhatsappClone.Shared.Extensions
    {
        public static class PrivacyRulesExtensions
        {
            public static string ResolveProfilePic(
                this string originalPicUrl,
                string privacyLevel,
                bool amIInHisContacts,
                bool amIExcluded)
            {
                return privacyLevel switch
                {
                    PrivacyLevel.Everyone => originalPicUrl,

                    PrivacyLevel.MyContacts when amIInHisContacts => originalPicUrl,

                    PrivacyLevel.MyContactsExcept when amIInHisContacts && !amIExcluded => originalPicUrl,

                    _ => "default_avatar_url"
                };
            }

            public static string CanViewLastSeen(this string about ,string privacyLevel, bool amIInHisContacts, bool amIExcluded)
            {
                return privacyLevel switch
                {
                    PrivacyLevel.Everyone => about,

                    PrivacyLevel.MyContacts when amIInHisContacts => about,

                    PrivacyLevel.MyContactsExcept when amIInHisContacts && !amIExcluded => about,

                    _ => ""
                };
            }
        }
    }
}
