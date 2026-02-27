using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Identity.Commands;

public class VerifyOtpCommand :IRequest<Response<VerifyOtpResult>>
{
    public string Email { get; set; }
    public string Otp {  get; set; }
}
