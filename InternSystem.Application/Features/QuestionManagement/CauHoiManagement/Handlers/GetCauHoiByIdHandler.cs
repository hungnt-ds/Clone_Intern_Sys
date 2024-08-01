using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Handlers
{
    public class GetCauHoiByIdHandler : IRequestHandler<GetCauHoiByIdQuery, GetCauHoiByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCauHoiByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCauHoiByIdResponse> Handle(GetCauHoiByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<CauHoi>();
                var userRepository = _unitOfWork.UserRepository;

                var cauHoiById = await repository
                    .GetAllQueryable()
                    .Include(ch => ch.CauHoiCongNghes)
                        .ThenInclude(chcn => chcn.CongNghe)
                    .FirstOrDefaultAsync(ch => ch.Id == request.Id && !ch.IsDelete);

                if( cauHoiById == null || cauHoiById.IsDelete)
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy câu hỏi."
                        );
                }

                var response = _mapper.Map<GetCauHoiByIdResponse>(cauHoiById);

                response.TenCongNghe = cauHoiById.CauHoiCongNghes.Select(chcn => chcn.CongNghe.Ten).ToList();

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
