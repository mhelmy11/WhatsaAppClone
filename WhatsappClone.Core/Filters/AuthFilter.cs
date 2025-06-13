using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // هنا ممكن نضيف منطق التحقق من الصلاحيات
            // مثلاً، لو المستخدم غير مسجل دخول، نرجع Unauthorized
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this resource.");

            }
            // لو في صلاحيات معينة لازم تتوفر، ممكن نتحقق منها هنا
            // else if (!context.HttpContext.User.HasClaim("Permission", "RequiredPermission"))
            // {
            //     context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
            // }

        }
    }
}
