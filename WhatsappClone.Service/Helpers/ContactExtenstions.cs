using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Infrastructure;

namespace WhatsappClone.Service.Helpers
{
    public static class ContactExtenstions
    {

        public static async Task<bool> CheckContactExistsAsync(
        this SqlDBContext db,
        string currentUserId,
        string targetPhoneNumber,
        string targetCountryCode,
        CancellationToken ct = default)
        {
            var contact =  await db.Users.FirstOrDefaultAsync(c => c.PhoneNumber == targetPhoneNumber && c.CountryCode == targetCountryCode);
            if ((contact is null))
            {
                return false;
            }

            return await db.Contacts.AnyAsync(u => u.UserId.ToString() == currentUserId && u.ContactUserId == contact.Id, ct);
        }
        public static async Task<bool> CheckContactExistsByIdAsync(
        this SqlDBContext db,
        string currentUserId,
        string targetUserId,
        CancellationToken ct = default)
        {
            return await db.Contacts.AnyAsync(u => u.UserId.ToString() == currentUserId && u.ContactUserId.ToString() == targetUserId, ct);
        }
    }
}
