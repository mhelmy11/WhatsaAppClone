using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
