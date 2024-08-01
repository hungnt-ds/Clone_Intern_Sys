using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Handlers
{
    public class GetCongNgheByIdHandler : IRequestHandler<GetCongNgheByIdQuery, GetCongNgheByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCongNgheByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCongNgheByIdResponse> Handle(GetCongNgheByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                CongNghe? existingCongNghe = await _unitOfWork.CongNgheRepository.GetCongNgheByIdAsync(request.Id)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Công nghệ không tồn tại.");
                var response = existingCongNghe.IsDelete == true
                    ? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ.")
                    : _mapper.Map<GetCongNgheByIdResponse>(existingCongNghe);

                response.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(response.CreatedBy!) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(response.LastUpdatedBy!) ?? "Người dùng không xác định";

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
