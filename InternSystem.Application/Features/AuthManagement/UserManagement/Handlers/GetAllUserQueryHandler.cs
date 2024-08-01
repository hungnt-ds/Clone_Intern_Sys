using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<GetAllUserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllUserResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userNetRepository = _unitOfWork.GetRepository<AspNetUser>();
                var userRepository = _unitOfWork.UserRepository;

                var listUser = await userNetRepository
                    .GetAllQueryable()
                    .Where(us => !us.IsDelete && us.IsActive)
                    .Include(u => u.UserDuAns)
                        .ThenInclude(us => us.DuAn)
                     .Include(u => u.UserViTris)
                        .ThenInclude(us => us.ViTri)
                     .Include(u => u.UserNhomZalos)
                        .ThenInclude(us => us.NhomZaloChung)
                     .Include(u => u.UserNhomZalos)
                        .ThenInclude(us => us.NhomZaloRieng)
                    .ToListAsync(cancellationToken);

                if (listUser == null || !listUser.Any() )
                {
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng.");
                }

                var response= _mapper.Map<IEnumerable<GetAllUserResponse>>(listUser);

                foreach ( var userResponse in response) {
                    if (!userResponse.CreatedBy.IsNullOrEmpty() && !userResponse.LastUpdatedBy.IsNullOrEmpty() )
                    {
                        userResponse.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(userResponse.CreatedBy) ?? "Người dùng không xác định";
                        userResponse.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(userResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                    }
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
