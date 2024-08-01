using AutoMapper;
using Azure;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Handlers
{
    public class GetUserNhomZaloByIdQueryHandler : IRequestHandler<GetUserNhomZaloByIdQuery, GetUserNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserNhomZaloByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserNhomZaloResponse> Handle(GetUserNhomZaloByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userNhomZaloRepository = _unitOfWork.GetRepository<UserNhomZalo>();
                var userRepository = _unitOfWork.UserRepository;
                var nhomZaloRepository = _unitOfWork.GetRepository<NhomZalo>();

                var userNhomZalo = await userNhomZaloRepository.GetAllQueryable()
                    .Where(us => (us.Id == request.Id) && !us.IsDelete)
                    .Include(unz => unz.User)
                    .FirstOrDefaultAsync(cancellationToken);
        
                if (userNhomZalo == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng trong nhóm Zalo");
                }

                var response = _mapper.Map<GetUserNhomZaloResponse>(userNhomZalo);
                response.TenNguoiDung = await userRepository.GetUserNameByIdAsync(userNhomZalo.UserId) ?? "Người dùng không xác định";
                var nhomZaloChung = await nhomZaloRepository.GetByIdAsync(response.IdNhomZaloChung);
                response.TenNhomZaloChung = nhomZaloChung.TenNhom;
                var nhomZaloRieng = await nhomZaloRepository.GetByIdAsync(response.IdNhomZaloRieng);
                response.TenNhomZaloRieng = nhomZaloRieng.TenNhom;

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
