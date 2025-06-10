using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoMediatoR;
using WhatsappClone.Core.Features.Chats.Queries.Models;
using WhatsappClone.Core.Features.Chats.Results;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMoMediatoR mediator;

        public ChatsController(IMoMediatoR mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var chats = await mediator.Send(new GetChatsQuery());
            return Ok(chats);
        }
    }

}
