using AutoMapper;
using Azure;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.CongNgheManagement.Queries;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Handlers
{
    public class GetPagedCongNgheQueryHandler : IRequestHandler<GetPagedCongNghesQuery, PaginatedList<GetPagedCongNghesResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedCongNgheQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedCongNghesResponse>> Handle(GetPagedCongNghesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<CongNghe>();
                var items = repository.GetAllQueryable().Include(cn => cn.ViTri);

                var paginatedItems = await PaginatedList<CongNghe>.CreateAsync(
                items,
                request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedCongNghesResponse>(item)).ToList();

                foreach (var congNgheResponse in responseItems)
                {
                    congNgheResponse.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(congNgheResponse.CreatedBy!) ?? "Người dùng không xác định";
                    congNgheResponse.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(congNgheResponse.LastUpdatedBy!) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedCongNghesResponse>(
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
