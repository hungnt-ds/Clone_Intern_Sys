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
    public class GetLichPhongvanByIdHandler : IRequestHandler<GetLichPhongVanByIdQuery, GetLichPhongVanByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLichPhongvanByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetLichPhongVanByIdResponse> Handle(GetLichPhongVanByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lichPhongVanRepository = _unitOfWork.GetRepository<LichPhongVan>();
                var userRepository = _unitOfWork.UserRepository;
                var internRepository = _unitOfWork.GetRepository<InternInfo>();

                var lichPhongVanById = await lichPhongVanRepository
                    .GetAllQueryable()
                    .Include(l => l.NguoiPhongVan)
                    .Include(l => l.NguoiDuocPhongVan)
                    .Where(l => l.IsActive && !l.IsDelete)
                    .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

                if (lichPhongVanById == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có lịch phỏng vấn."
                    );
                }

                var response = _mapper.Map<GetLichPhongVanByIdResponse>(lichPhongVanById);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";

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
