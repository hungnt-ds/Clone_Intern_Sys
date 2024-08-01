using AutoMapper;
using Azure;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Handlers
{
    public class GetPagedCauHoiCongNgheQueryHandler : IRequestHandler<GetPagedCauHoiCongNgheQuery, PaginatedList<GetPagedCauHoiCongNghesResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedCauHoiCongNgheQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedCauHoiCongNghesResponse>> Handle(GetPagedCauHoiCongNgheQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<CauHoiCongNghe>();
                var items = repository.GetAllQueryable()
                    .Include(chcn => chcn.CauHoi)
                    .Include(chcn => chcn.CongNghe)
                    .Where(da => da.IsActive && !da.IsDelete);

                var paginatedItems = await PaginatedList<CauHoiCongNghe>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items
                    .Select(item => _mapper.Map<GetPagedCauHoiCongNghesResponse>(item))
                    .ToList();

                var responsePaginatedList = new PaginatedList<GetPagedCauHoiCongNghesResponse>(
                    responseItems,
                    paginatedItems.TotalCount,
                    paginatedItems.PageNumber,
                    paginatedItems.PageSize
                );
                foreach (var cauHoiCongNgheResponse in responseItems)
                {
                    cauHoiCongNgheResponse.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(cauHoiCongNgheResponse.CreatedBy) ?? "Người dùng không xác định";
                    cauHoiCongNgheResponse.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(cauHoiCongNgheResponse.LastUpdatedBy) ?? "Người dùng không xác định";

                }
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
