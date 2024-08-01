using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Handlers
{
    public class GetPagedPhongVanQueryHandler : IRequestHandler<GetPagedPhongVanQuery, PaginatedList<GetPagedPhongVansResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedPhongVanQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedPhongVansResponse>> Handle(GetPagedPhongVanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var phongVanRepository = _unitOfWork.GetRepository<PhongVan>();
                var userRepository = _unitOfWork.UserRepository;

                var phongVanQuery = phongVanRepository
                    .GetAllQueryable()
                    .Include(pv => pv.CauHoiCongNghe)
                        .ThenInclude(chcn => chcn.CauHoi)
                    .Include(pv => pv.CauHoiCongNghe)
                        .ThenInclude(chcn => chcn.CongNghe)
                    .Include(pv => pv.LichPhongVan)
                        .ThenInclude(lpv => lpv.NguoiPhongVan)
                    .Where(pv => pv.IsActive && !pv.IsDelete);

                var paginatedItems = await PaginatedList<PhongVan>.CreateAsync(
                    phongVanQuery,
                    request.PageNumber,
                    request.PageSize
                );

                if (!paginatedItems.Items.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có phỏng vấn."
                    );
                }

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedPhongVansResponse>(item)).ToList();

                foreach (var response in responseItems)
                {
                    response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                    response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedPhongVansResponse>(
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
