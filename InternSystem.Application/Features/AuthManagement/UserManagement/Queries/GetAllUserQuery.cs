﻿using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Queries
{
    public class GetAllUserQuery : IRequest<IEnumerable<GetAllUserResponse>>
    {
    }
}
