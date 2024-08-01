using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Handlers
{
    public class GetUserViTriByIdHandler : IRequestHandler<GetUserViTriByIdQuery, GetUserViTriByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserViTriByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserViTriByIdResponse> Handle(GetUserViTriByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                UserViTri? existingUserViTri = await _unitOfWork.UserViTriRepository.GetByIdAsync(request.Id);
                    
                if (existingUserViTri == null || existingUserViTri.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng này trong vị trí này");
            
               var response = _mapper.Map<GetUserViTriByIdResponse>(existingUserViTri);

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
