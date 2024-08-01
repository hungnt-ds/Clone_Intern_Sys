using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Handlers
{
    public class GetAllLichPhongVanHandler : IRequestHandler<GetAllLichPhongVanQuery, IEnumerable<GetAllLichPhongVanResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLichPhongVanHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllLichPhongVanResponse>> Handle(GetAllLichPhongVanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lichPhongVanRepository = _unitOfWork.GetRepository<LichPhongVan>();
                var userRepository = _unitOfWork.UserRepository;
                var internRepository = _unitOfWork.GetRepository<InternInfo>();
                
                var listLichPhongVan = await lichPhongVanRepository
                    .GetAllQueryable()
                    .Include(l => l.NguoiPhongVan)
                    .Include(l => l.NguoiDuocPhongVan)
                    .Where(l => l.IsActive && !l.IsDelete)
                    .ToListAsync(cancellationToken);

                if (listLichPhongVan == null || !listLichPhongVan.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có lịch phỏng vấn."
                    );
                }

                var response = _mapper.Map<IEnumerable<GetAllLichPhongVanResponse>>(listLichPhongVan);

                foreach (var lichPhongVanResponse in response)
                {
                    lichPhongVanResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(lichPhongVanResponse.CreatedBy) ?? "Người dùng không xác định";
                    lichPhongVanResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(lichPhongVanResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                return response;
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
