using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Handlers
{
    public class GetPagedUserViTriQueryHandler : IRequestHandler<GetPagedUserViTriQuery, PaginatedList<GetPagedUserViTriResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedUserViTriQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedUserViTriResponse>> Handle(GetPagedUserViTriQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<UserViTri>();
                var items = repository.GetAllQueryable()
                    .Include(uv => uv.AspNetUser)
                    .Include(uv => uv.ViTri)
                    .Where(uv => uv.IsActive && !uv.IsDelete);
                   
                var paginatedItems = await PaginatedList<UserViTri>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedUserViTriResponse>(item)).ToList();

                foreach (var item in responseItems)
                {
                    item.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(item.CreatedBy!) ?? "Người dùng không xác định";
                    item.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(item.LastUpdatedBy!) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedUserViTriResponse>(
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
