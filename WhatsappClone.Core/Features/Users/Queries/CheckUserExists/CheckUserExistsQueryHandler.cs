using MediatR;
using Microsoft.AspNetCore.Identity;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Queries
{
    //SignalR endpoint
    //it will be used in Hub
    public class CheckUserExistsQueryHandler : ResponseHandler, IRequestHandler<CheckUserExistsQuery, Response<bool>>
    {
        private readonly UserManager<User> userManager;
        private readonly PhoneNumberService phoneNumberService;

        public CheckUserExistsQueryHandler(UserManager<User> userManager, PhoneNumberService phoneNumberService)
        {
            this.userManager = userManager;
            this.phoneNumberService = phoneNumberService;
        }
        public async Task<Response<bool>> Handle(CheckUserExistsQuery request, CancellationToken cancellationToken)
        {

            var (cleanCountryCode, cleanNationalNumber) = phoneNumberService.CleanPhoneNumber(request.CountryCode, request.PhoneNumber);


            var userFromDb = await userManager.FindByPhoneNumber(cleanCountryCode, cleanNationalNumber);
            if (userFromDb == null)
            {
                return Success(false, "This phone number is not on Whatsapp");
            }

            return Success(true, "This phone number is on Whatsapp");


        }
    }
}
