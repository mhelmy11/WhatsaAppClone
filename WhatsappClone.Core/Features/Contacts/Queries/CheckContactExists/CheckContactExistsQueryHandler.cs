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
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Contacts.Queries
{
    public class CheckContactExistsQueryHandler : ResponseHandler , IRequestHandler<CheckContactExistsQuery, Response<bool>>
    {
        private readonly SqlDBContext dBContext;
        private readonly PhoneNumberService phoneNumberService;
        private readonly ICurrentUserService currentUserService;

        public CheckContactExistsQueryHandler(
            SqlDBContext dBContext,
            PhoneNumberService phoneNumberService,
            ICurrentUserService currentUserService
            )
        {
            this.dBContext = dBContext;
            this.phoneNumberService = phoneNumberService;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<bool>> Handle(CheckContactExistsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var (cleanedCountryCode, cleanedNationalNumber) = phoneNumberService.CleanPhoneNumber(request.CountryCode, request.PhoneNumber);
            var isContact = await dBContext.CheckContactExistsAsync(currentUserId.ToString(), cleanedNationalNumber, cleanedCountryCode ,cancellationToken);

            return Success(isContact);
        }
    }
}
