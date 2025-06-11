using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsappClone.API.Base;

//using MoMediatoR;
using WhatsappClone.Core.Features.Chats.Queries.Models;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : AppControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await mediator.Send(new GetChatsQuery());
            return ResponseResult(chats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {
            throw new ValidationException(new List<ValidationFailure>() { });

            var chat = await mediator.Send(new GetChatByIdQuery(id));

            return ResponseResult(chat);

        }
    }

}
