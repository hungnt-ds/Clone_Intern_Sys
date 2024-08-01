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
    public class GetAllThongBaoHandler : IRequestHandler<GetAllThongBaoQuery, IEnumerable<GetAllThongBaoResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllThongBaoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllThongBaoResponse>> Handle(GetAllThongBaoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var thongBaoRepository = _unitOfWork.GetRepository<ThongBao>();
                var userRepository = _unitOfWork.UserRepository;

                var response = await thongBaoRepository
                    .GetAllQueryable()
                    .Include(tb => tb.NguoiNhan) 
                    .Include(tb => tb.NguoiGui)
                    .Where(da => da.IsActive && !da.IsDelete)
                    .ToListAsync(cancellationToken);

                if (response == null || !response.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy kết quả");
                }

                var mappedResponse = _mapper.Map<IEnumerable<GetAllThongBaoResponse>>(response);
                foreach (var thongBaoResponse in mappedResponse)
                {
                    thongBaoResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(thongBaoResponse.CreatedBy) ?? "Người dùng không xác định";
                    thongBaoResponse.LastUpdatedName = await userRepository.GetUserNameByIdAsync(thongBaoResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }
                return mappedResponse;
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
