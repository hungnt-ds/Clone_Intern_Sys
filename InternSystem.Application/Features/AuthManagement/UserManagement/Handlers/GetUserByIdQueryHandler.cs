using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.AuthManagement.UserManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserDetailResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<GetUserDetailResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }

                return new GetUserDetailResponse
                {
                    HoVaTen = user.HoVaTen ?? string.Empty,
                    ImageUrl = user.ImagePath ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty
                };
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
