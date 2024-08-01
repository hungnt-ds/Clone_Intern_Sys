using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Handlers
{
    public class GetPagedThongBaosQueryHandler : IRequestHandler<GetPagedThongBaosQuery, PaginatedList<GetPagedThongBaosResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedThongBaosQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedThongBaosResponse>> Handle(GetPagedThongBaosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var thongBaoRepository = _unitOfWork.GetRepository<ThongBao>();
                var userRepository = _unitOfWork.UserRepository;

                var query = thongBaoRepository.GetAllQueryable()
                    .Include(tb => tb.NguoiNhan)
                    .Include(tb => tb.NguoiGui)
                    .Where(tb => tb.IsActive && !tb.IsDelete);

                var paginatedItems = await PaginatedList<ThongBao>.CreateAsync(
                    query,
                    request.PageNumber,
                    request.PageSize
         
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedThongBaosResponse>(item)).ToList();

                foreach (var thongBaoResponse in responseItems)
                {
                    thongBaoResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(thongBaoResponse.CreatedBy) ?? "Người dùng không xác định";
                    thongBaoResponse.LastUpdatedName = await userRepository.GetUserNameByIdAsync(thongBaoResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedThongBaosResponse>(
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
