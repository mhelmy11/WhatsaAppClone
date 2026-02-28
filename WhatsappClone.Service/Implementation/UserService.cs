using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public static class UserService
    {

        public static User FindByPhoneNumber(this UserManager<User> userManager, string PhoneNumber)
        {
            var User = userManager.Users.FirstOrDefault(u => u.PhoneNumber == PhoneNumber);

            return User;
        }
        public static async Task<User> GetCurrentUser(this UserManager<User> userManager , IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) {

                throw new UnauthorizedException("Unauthorized Access");
                
            }
            return await userManager.FindByIdAsync(userId);
        }
    }
}
