using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using MediatR;
using WhatsappClone.Core.Features.Chats.Queries.Results;
using WhatsappClone.Data.Enums;
using WhatsappClone.Core.Wrappers;

namespace WhatsappClone.Core.Features.Chats.Queries.Models;

public class GetPaginatedChatsQuery : IRequest<PaginatedResult<GetPaginatedChatsResponse>>
{
    public int pageNumber { get; set; }
    public int pageSize { get; set; }

    public ChatOrderingEnum? orderBy { get; set; }

    public string? search { get; set; }

    public GetPaginatedChatsQuery(int pageNumber, int pageSize, ChatOrderingEnum? orderBy, string search)
    {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.orderBy = orderBy;
        this.search = search;

    }
    public GetPaginatedChatsQuery()
    {

    }
}
