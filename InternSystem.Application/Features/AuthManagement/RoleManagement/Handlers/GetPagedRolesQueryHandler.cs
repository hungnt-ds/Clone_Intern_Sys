using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Handlers
{
    public class GetPagedRolesQueryHandler : IRequestHandler<GetPagedRolesQuery, PaginatedList<GetPagedRolesResponse>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public GetPagedRolesQueryHandler(IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<PaginatedList<GetPagedRolesResponse>> Handle(GetPagedRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var items = _roleManager.Roles.AsQueryable();

                var paginatedItems = await PaginatedList<IdentityRole>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedRolesResponse>(item)).ToList();

                var responsePaginatedList = new PaginatedList<GetPagedRolesResponse>(
                    responseItems,
                    paginatedItems.TotalCount,
                    paginatedItems.PageNumber,
                    paginatedItems.PageSize
                );

                return responsePaginatedList;
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
