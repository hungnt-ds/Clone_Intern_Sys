using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class GetPagedUsersQueryHandler : IRequestHandler<GetPagedUsersQuery, PaginatedList<GetPagedUsersResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedUsersQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedUsersResponse>> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AspNetUser>();
                var items = repository.GetAllQueryable()
                    .Where(us => us.IsActive && !us.IsDelete)
                    .Include(u => u.UserDuAns)
                        .ThenInclude(us => us.DuAn)
                     .Include(u => u.UserViTris)
                        .ThenInclude(us => us.ViTri)
                     .Include(u => u.UserNhomZalos)
                        .ThenInclude(us => us.NhomZaloChung)
                     .Include(u => u.UserNhomZalos)
                        .ThenInclude(us => us.NhomZaloRieng);
                if (!items.Any() || items == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có người dùng."
                    );
                }

                var paginatedItems = await PaginatedList<AspNetUser>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedUsersResponse>(item)).ToList();

                foreach (var item in responseItems)
                {
                    item.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(item.CreatedBy) ?? "Người dùng không xác định";
                    item.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(item.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedUsersResponse>(
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
