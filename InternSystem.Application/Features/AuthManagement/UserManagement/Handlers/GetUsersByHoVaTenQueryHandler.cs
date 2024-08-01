using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Queries;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class GetUsersByHoVaTenQueryHandler : IRequestHandler<GetUsersByHoVaTenQuery, IEnumerable<GetAllUserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersByHoVaTenQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllUserResponse>> Handle(GetUsersByHoVaTenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userNetRepository = _unitOfWork.GetRepository<AspNetUser>();
                var userRepository = _unitOfWork.UserRepository;

                var listUser = await userNetRepository
                    .GetAllQueryable()
                    .Where(us => !us.IsDelete && us.IsActive && us.HoVaTen.Trim().ToLower().Contains(request.HoVaTen.Trim().ToLower()))
                    .Include(u => u.UserDuAns)
                        .ThenInclude(us => us.DuAn)
                     .Include(u => u.UserViTris)
                        .ThenInclude(us => us.ViTri)
                     .Include(u => u.UserNhomZalos)
                        .ThenInclude(us => us.NhomZaloChung)
                     .Include(u => u.UserNhomZalos)
                        .ThenInclude(us => us.NhomZaloRieng)
                    .ToListAsync(cancellationToken);

                if (listUser == null || !listUser.Any())
                {
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng.");
                }

                var response = _mapper.Map<IEnumerable<GetAllUserResponse>>(listUser);

                foreach (var userResponse in response)
                {
                    // comment lại vì có mapper làm cho rồi -- để đây để tham khảo

                    // Lấy các thuộc tính tên cách dự án, nhóm zalo đã tham gia, tên vị trí của người dùng
                    //var user = listUser.FirstOrDefault(u => u.Id == userResponse.Id);
                    //if (user != null)
                    //{
                    //    userResponse.ListDuAn = user.UserDuAns
                    //        .Where(ud => ud.DuAn != null)
                    //        .Select(ud => ud.DuAn.Ten)
                    //        .ToList();

                    //    userResponse.ListViTri = user.UserViTris
                    //        .Where(uv => uv.ViTri != null)
                    //        .Select(uv => uv.ViTri.Ten)
                    //        .ToList();

                    //    userResponse.ListNhomZalo = user.UserNhomZalos
                    //        .Where(unz => unz.NhomZaloChung != null)
                    //        .Select(unz => unz.NhomZaloChung.TenNhom)
                    //        .Concat(user.UserNhomZalos
                    //            .Where(unz => unz.NhomZaloRieng != null)
                    //            .Select(unz => unz.NhomZaloRieng.TenNhom))
                    //        .ToList();
                    //}

                    if (!userResponse.CreatedBy.IsNullOrEmpty() && !userResponse.LastUpdatedBy.IsNullOrEmpty())
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
