using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Handlers
{
    public class GetUserDuAnByIdHandler : IRequestHandler<GetUserDuAnByIdQuery, GetUserDuAnByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserDuAnByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserDuAnByIdResponse> Handle(GetUserDuAnByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //UserDuAn? existingUserDA = await _unitOfWork.UserDuAnRepository.GetByIdAsync(request.Id);
                //if (existingUserDA == null || existingUserDA.IsDelete == true)
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án");

                //return _mapper.Map<GetUserDuAnByIdResponse>(existingUserDA);
                var repository = _unitOfWork.GetRepository<UserDuAn>();
                var userRepository = _unitOfWork.UserRepository;

                var userDuAnById = await repository
                    .GetAllQueryable()
                    .FirstOrDefaultAsync(uda => uda.Id == request.Id && !uda.IsDelete);

                if(userDuAnById == null || userDuAnById.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng của Dự Án.");
                }

                var response = _mapper.Map<GetUserDuAnByIdResponse>(userDuAnById);

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
