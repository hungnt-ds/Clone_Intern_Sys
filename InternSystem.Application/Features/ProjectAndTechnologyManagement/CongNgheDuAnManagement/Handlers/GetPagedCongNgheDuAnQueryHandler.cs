using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.CongNgheDuAnManagement.Handlers
{
    public class GetPagedCongNgheDuAnQueryHandler : IRequestHandler<GetPagedCongNgheDuAnQuery, PaginatedList<GetPagedCongNgheDuAnsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedCongNgheDuAnQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedCongNgheDuAnsResponse>> Handle(GetPagedCongNgheDuAnQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var congNgheDuAnRepository = _unitOfWork.GetRepository<CongNgheDuAn>();

                var items = congNgheDuAnRepository.GetAllQueryable()
                    .Include(cnda => cnda.CongNghe)
                    .Include(cnda => cnda.DuAn)
                    .Where(cnda => cnda.IsActive && !cnda.IsDelete);

                var paginatedItems = await PaginatedList<CongNgheDuAn>.CreateAsync(
                    items, request.PageNumber, request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(async item =>
                {
                    var responseItem = _mapper.Map<GetPagedCongNgheDuAnsResponse>(item);
                    responseItem.TenCongNghe = item.CongNghe?.Ten;
                    responseItem.TenDuAn = item.DuAn?.Ten;
                   return responseItem;
                }).ToList();

                var responsePaginatedList = new PaginatedList<GetPagedCongNgheDuAnsResponse>(
                    await Task.WhenAll(responseItems), paginatedItems.TotalCount, paginatedItems.PageNumber, paginatedItems.PageSize
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
