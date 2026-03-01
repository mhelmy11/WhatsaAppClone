using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Contacts.Queries
{
    public class CheckContactExistsQueryHandler : ResponseHandler , IRequestHandler<CheckContactExistsQuery, Response<bool>>
    {
        private readonly SqlDBContext dBContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly PhoneNumberService phoneNumberService;

        public CheckContactExistsQueryHandler(SqlDBContext dBContext , IHttpContextAccessor httpContextAccessor,UserManager<User> userManager , PhoneNumberService phoneNumberService )
        {
            this.dBContext = dBContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.phoneNumberService = phoneNumberService;
        }
        public async Task<Response<bool>> Handle(CheckContactExistsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);
            var (cleanedCountryCode, cleanedNationalNumber) = phoneNumberService.CleanPhoneNumber(request.CountryCode, request.PhoneNumber);
            var isContact = await dBContext.CheckContactExistsAsync(currentUser.Id.ToString(), cleanedNationalNumber, cleanedCountryCode ,cancellationToken);

            return Success(isContact);
        }
    }
}
