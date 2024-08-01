using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Queries
{
     public class GetRoleByIdQuery : IRequest<IEnumerable<GetRoleResponse>>
    {
        public string RoleId;
    }
}
