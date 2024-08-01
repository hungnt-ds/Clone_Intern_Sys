using AutoMapper;
using InternSystem.Application.Common.Constants;
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
    public class GetThongBaoByIdHandler : IRequestHandler<GetThongBaoByIdQuery, GetThongBaoByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetThongBaoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetThongBaoByIdResponse> Handle(GetThongBaoByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var thongBaoRepository = _unitOfWork.GetRepository<ThongBao>();
                var userRepository = _unitOfWork.UserRepository;

                var result = await thongBaoRepository
                    .GetAllQueryable()
                    .Include(tb => tb.NguoiNhan) 
                    .Include(tb => tb.NguoiGui)
                    .Where(da => da.IsActive && !da.IsDelete)
                    .FirstOrDefaultAsync(tb => tb.Id == request.Id, cancellationToken);

                if (result == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông báo");
                }

                var response = _mapper.Map<GetThongBaoByIdResponse>(result);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";
                
                return response;
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }

    }
}
