using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using MoMediatoR;
using WhatsappClone.Core.Features.Chats.Queries.Models;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ChatsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await mediator.Send(new GetChatsQuery());
            return Ok(chats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {

            var chat = await mediator.Send(new GetChatByIdQuery(id));

            return Ok(chat);

        }
    }

}
