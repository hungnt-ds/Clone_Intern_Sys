using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Handlers
{
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, IEnumerable<GetRoleResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        public GetRoleByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetRoleResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get all roles using RoleManager
                var roles = await _roleManager.Roles
                    .Where(rl => rl.Id == request.RoleId)
                    .ToListAsync();
                if (!roles.Any() || roles == null) {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy role.");
                }

                // Map roles to GetRoleResponse objects
                return _mapper.Map<IEnumerable<GetRoleResponse>>(roles);
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
