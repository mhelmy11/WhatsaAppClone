using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public static async Task<User> FindByPhoneNumber(this UserManager<User> userManager, string CountryCode, string PhoneNumber)
        {
            var User = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber && u.CountryCode == CountryCode);

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
