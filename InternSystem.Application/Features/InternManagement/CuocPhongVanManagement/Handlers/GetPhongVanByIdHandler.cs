using AutoMapper;
using InternSystem.Application.Common.Constants;
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
    public class GetPhongVanByIdHandler : IRequestHandler<GetPhongVanByIdQuery, GetPhongVanByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPhongVanByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetPhongVanByIdResponse> Handle(GetPhongVanByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var phongVanRepository = _unitOfWork.GetRepository<PhongVan>();
                var userRepository = _unitOfWork.UserRepository;

                var phongVanById = await phongVanRepository
                    .GetAllQueryable()
                    .Include(pv => pv.CauHoiCongNghe)
                        .ThenInclude(chcn => chcn.CauHoi)
                    .Include(pv => pv.CauHoiCongNghe)
                        .ThenInclude(chcn => chcn.CongNghe)
                    .Include(pv => pv.LichPhongVan)
                        .ThenInclude(lpv => lpv.NguoiPhongVan)
                    .Where(pv => pv.IsActive && !pv.IsDelete)
                    .FirstOrDefaultAsync(pv => pv.Id == request.Id, cancellationToken);

                if (phongVanById == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có phỏng vấn."
                    );
                }

                var response = _mapper.Map<GetPhongVanByIdResponse>(phongVanById);

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
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lưu.");
            }         
        }
    }
}
