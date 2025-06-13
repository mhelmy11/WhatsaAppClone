using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : AppControllerBase
    {



        [HttpPost]
        public async Task<IActionResult> Register([FromForm] AddUserCommand command, [FromServices] IFileService fileService)
        {

            #region old
            //var ProfilePic = command.ProfilePic; //Request.Form.Files["ProfilePic"];
            //var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            //if (!Directory.Exists(uploadsFolder))
            //{
            //    Directory.CreateDirectory(uploadsFolder);
            //}

            //var filePath = Path.Combine(uploadsFolder, Guid.NewGuid().ToString() + Path.GetExtension(ProfilePic.FileName).ToLowerInvariant());

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await ProfilePic.CopyToAsync(stream);
            //}

            #endregion

            if (command.ProfilePic != null)
            {
                var profilePicPath = await fileService.SaveFileAsync(command.ProfilePic, "ProfilePics");

                command.PicUrl = profilePicPath;
            }

            var result = await mediator.Send(command);

            return ResponseResult(result);
        }
    }
}
