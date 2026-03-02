using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WhatsappClone.Core.Features.Users.Queries;

public class GetProfileResult
{
    public string Name { get; set; }
    public string ProfilePic {  get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }

    public string About { get; set; }
    public bool IsContact { get; set; } = true;
}
