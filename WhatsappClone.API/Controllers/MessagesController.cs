using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Features.Messages.Commands.SendMessage;
namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : AppControllerBase
    {


        [HttpPost("send-message")]

        public async Task<IActionResult> SendMessage( SendMessageCommand command)
        {
            var result = await mediator.Send(command);
            return ResponseResult(result);
        }

    }
}
