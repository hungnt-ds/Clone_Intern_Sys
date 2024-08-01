using AutoMapper;
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
    public class GetAllUserNhomZaloQueryHandler : IRequestHandler<GetAllUserNhomZaloQuery, IEnumerable<GetUserNhomZaloResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUserNhomZaloQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUserNhomZaloResponse>> Handle(GetAllUserNhomZaloQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userNhomZaloRepository = _unitOfWork.GetRepository<UserNhomZalo>();
                var userRepository = _unitOfWork.UserRepository;
                var nhomZaloRepository = _unitOfWork.GetRepository<NhomZalo>();

                var userNhomZalos = await userNhomZaloRepository.GetAllQueryable()
                    .Where(p => !p.IsDelete)
                    .OrderBy(p => p.UserId)
                    .ToListAsync(cancellationToken);

                if (!userNhomZalos.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy người dùng trong nhóm Zalo"
                    );
                }

                var response = _mapper.Map<IEnumerable<GetUserNhomZaloResponse>>(userNhomZalos);

                foreach (var item in response)
                {
                    item.TenNguoiDung = await userRepository.GetUserNameByIdAsync(item.UserId) ?? "Người dùng không xác định";
                    var nhomZaloChung = await nhomZaloRepository.GetByIdAsync(item.IdNhomZaloChung);
                    var nhomZaloRieng = await nhomZaloRepository.GetByIdAsync(item.IdNhomZaloRieng);
                    item.TenNhomZaloChung = nhomZaloChung.TenNhom;
                    item.TenNhomZaloRieng = nhomZaloRieng.TenNhom;
                }

                return response;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(
                    StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    "Đã có lỗi xảy ra"
                );
            }
        }
    }
}
