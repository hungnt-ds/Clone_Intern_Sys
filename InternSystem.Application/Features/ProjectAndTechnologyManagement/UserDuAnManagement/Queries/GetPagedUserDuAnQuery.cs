using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries
{
    public class GetPagedUserDuAnQuery : IRequest<PaginatedList<GetPagedUserDuAnResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPagedUserDuAnQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
