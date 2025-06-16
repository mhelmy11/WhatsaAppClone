using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.RequirementsHandlers;

namespace WhatsappClone.API.Requirements.Handlers
{

    public class SessionNotRevokedRequirement : IAuthorizationRequirement
    {
    }

    public class SessionNotRevokedHandler : AuthorizationHandler<SessionNotRevokedRequirement>
    {
        private readonly SeesionNotRevokedRequirementHandler requirementHandler;

        public SessionNotRevokedHandler(SeesionNotRevokedRequirementHandler requirementHandler)
        {
            this.requirementHandler = requirementHandler;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SessionNotRevokedRequirement requirement)
        {


            var tid = context.User.FindFirst("TID");
            if (tid == null)
            {
                // إذا لم يكن هناك sid، افشل في التحقق
                context.Fail();
                return;
            }


            // تحقق من وجود الـ sid في قائمة Redis السوداء
            var isValid = await requirementHandler.HandleAsync(int.Parse(tid.Value));

            // إذا لم يكن ملغياً، فالشرط قد تحقق
            if (isValid)
            {
                context.Succeed(requirement);
            }
            else
            {
                // إذا كان ملغياً، افشل بصمت (الفشل هو الحالة الافتراضية إذا لم يتم استدعاء Succeed)
                context.Fail();
            }
        }
    }
}
