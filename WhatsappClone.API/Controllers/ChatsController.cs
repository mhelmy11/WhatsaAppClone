using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatsappClone.API.Base;
using WhatsappClone.Core.Filters;

//using MoMediatoR;
using WhatsappClone.Core.Features.Chats.Queries.Models;

namespace WhatsappClone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : AppControllerBase
    {




        [HttpGet("Chats")]
        [Authorize]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await mediator.Send(new GetChatListQuery());
            return ResponseResult(chats);
        }




        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetChats()
        {
            var chats = await mediator.Send(new GetChatsQuery());
            return ResponseResult(chats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {

            var chat = await mediator.Send(new GetChatByIdQuery(id));

            return ResponseResult(chat);

        }

        [HttpGet("Paginated")]
        public async Task<IActionResult> GetPaginatedChats([FromQuery] GetPaginatedChatsQuery query)
        {
            var paginatedChats = await mediator.Send(query);
            return ResponseResult(paginatedChats);
        }
    }

}
