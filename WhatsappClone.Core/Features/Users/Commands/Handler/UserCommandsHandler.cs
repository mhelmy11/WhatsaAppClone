using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Commands.Handler
{
    public class UserCommandsHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<string>>
                                                      , IRequestHandler<ForgetPasswordCommand, Response<string>>
                                                      , IRequestHandler<ResetPasswordCommand, Response<string>>
                                                      , IRequestHandler<EditMeCommand, Response<string>>
    {
        private readonly IFileService fileService;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContext;
        private readonly IEmailService emailService;
        private readonly ITransactionService transactionService;

        public UserCommandsHandler(IFileService fileService, IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContext, IEmailService emailService, ITransactionService transactionService)
        {
            this.fileService = fileService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.httpContext = httpContext;
            this.emailService = emailService;
            this.transactionService = transactionService;
        }
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {



            // Start a transaction
            var transaction = transactionService.BeginTransaction();
            var error = string.Empty;
            try
            {

                var newUser = mapper.Map<AppUser>(request);

                if (request.ProfilePic != null)
                {
                    // Save the profile picture and get the URL
                    var picUrl = await fileService.SaveFileAsync(request.ProfilePic, "ProfilePics");
                    newUser.PicUrl = picUrl;
                }


                var Result = await userManager.CreateAsync(newUser, request.Password);

                if (Result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); //important to avoid "/ or +" in generated token
                    var requestContext = httpContext.HttpContext!.Request;
                    var confirmationLink = $"{requestContext.Scheme}://{requestContext.Host}/api/authentication/confirm-email?Id={newUser.Id}&token={encodedToken}";
                    var subject = "Confirm Your Email";
                    var htmlContent = $"<h1>Welcome!</h1><p>Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.</p>";
                    await emailService.SendEmailAsync(newUser.Email!, subject, htmlContent);
                    transaction.Commit();
                    return Success<string>("Registration successful. Please check your email to confirm your account.");
                }
                error = Result.Errors.Select(x => x.Description).FirstOrDefault() ?? "";
            }

            catch
            {
                transaction.Rollback();
            }

            return BadRequest<string>(error);





        }

        public async Task<Response<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(request.Email));
                var requestContext = httpContext.HttpContext!.Request;
                var confirmationLink = $"{requestContext.Scheme}://Whatsapp/reset-password?Email={encodedEmail}&ResetToken={encodedToken}";
                var subject = "Reset Password";
                var htmlContent = $"<h1>Welcome!</h1><p>reset password by <a href='{confirmationLink}'>clicking here</a>.</p>";
                await emailService.SendEmailAsync(request.Email, subject, htmlContent);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }

            return Success<string>("Reset password email sent successfully. Please check your email to reset your password.");



        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetToken));
                var decodedEmail = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Email));
                var user = await userManager.FindByEmailAsync(decodedEmail);

                var result = await userManager.ResetPasswordAsync(user, decodedToken, request.Password);

                if (result.Succeeded)
                {
                    // تم تغيير كلمة المرور بنجاح
                    return Success<string>("Password reset successful.");
                }

                var error = result.Errors.Select(e => e.Description).FirstOrDefault() ?? "";
                return BadRequest<string>(error);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }

        }

        public async Task<Response<string>> Handle(EditMeCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return BadRequest<string>("User not found.");
            }

            var updatedUser = mapper.Map(request, user);

            if (request.ProfilePic != null)
            {
                // Save the profile picture and get the URL
                var picUrl = await fileService.SaveFileAsync(request.ProfilePic, "ProfilePics");
                updatedUser.PicUrl = picUrl;
            }

            var result = await userManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                return Success<string>("User updated successfully.");
            }
            var error = result.Errors.Select(e => e.Description).FirstOrDefault() ?? "";
            return BadRequest<string>(error);



        }
    }
}
