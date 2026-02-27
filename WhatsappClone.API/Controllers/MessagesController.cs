using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WhatsappClone.API.Base;
namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : AppControllerBase
    {


        //[HttpPost("send-group-message")]
        //[Authorize]


        //public async Task<IActionResult> SendGroupMessage([FromForm] SendGroupMessageCommand command)
        //{
        //    var result = await mediator.Send(command);
        //    return ResponseResult(result);
        //}

    }
}
