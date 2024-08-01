using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.CauHoiManagement.Handlers
{
    public class GetPagedDuAnQueryHandler : IRequestHandler<GetPagedDuAnQuery, PaginatedList<GetPagedDuAnsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedDuAnQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedDuAnsResponse>> Handle(GetPagedDuAnQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var duAnRepository = _unitOfWork.GetRepository<DuAn>();
                var userRepository = _unitOfWork.UserRepository;

                var query = duAnRepository.GetAllQueryable()
                    .Include(da => da.Leader)
                    .Include(da => da.CongNgheDuAns)
                        .ThenInclude(cnda => cnda.CongNghe)
                    .Where(da => da.IsActive && !da.IsDelete);

                var paginatedItems = await PaginatedList<DuAn>.CreateAsync(
                    query,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedDuAnsResponse>(item)).ToList();

                foreach (var duAnResponse in responseItems)
                {
                    duAnResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                    duAnResponse.LastUpdatedName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedDuAnsResponse>(
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
