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
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // هنا ممكن نضيف منطق التحقق من الصلاحيات
            if (!context.HttpContext.User.HasClaim("Permission", "RequiredPermission"))
            {
                context.Result = new ForbidResult();
            }

        }
    }
}
