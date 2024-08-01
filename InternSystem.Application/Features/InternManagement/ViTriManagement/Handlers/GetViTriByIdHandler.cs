using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Handlers
{
    public class GetUserViTriByIdHandler : IRequestHandler<GetViTriByIdQuery, GetViTriByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserViTriByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetViTriByIdResponse> Handle(GetViTriByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //ViTri? existingViTri = await _unitOfWork.ViTriRepository.GetByIdAsync(request.Id);
                //if (existingViTri == null || existingViTri.IsDelete)
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");

                //return _mapper.Map<GetViTriByIdResponse>(existingViTri);
                var repository = _unitOfWork.GetRepository<ViTri>();
                var userRepository = _unitOfWork.UserRepository;

                var viTriById = await repository
                    .GetAllQueryable()
                    .FirstOrDefaultAsync(vt => vt.Id == request.Id && !vt.IsDelete);

                if(viTriById == null || viTriById.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí.");
                }

                var response = _mapper.Map<GetViTriByIdResponse>(viTriById);

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
