using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Service.Implementation
{
    public class PhoneNumberService
    {
       public (string CountryCode,string PhoneNumber) CleanPhoneNumber (string CountryCode,string PhoneNumber)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var parsedNumber = phoneUtil.Parse($"+{CountryCode + PhoneNumber}", null);
            string cleanCountryCode = $"{parsedNumber.CountryCode}";
            string cleanNationalNumber = parsedNumber.NationalNumber.ToString();
            return (cleanCountryCode , cleanNationalNumber);
        }
    }
}
