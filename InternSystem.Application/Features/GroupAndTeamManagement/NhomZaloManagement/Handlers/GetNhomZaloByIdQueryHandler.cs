using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Handlers
{
    public class GetNhomZaloByIdQueryHandler : IRequestHandler<GetNhomZaloByIdQuery, GetNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNhomZaloByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetNhomZaloResponse> Handle(GetNhomZaloByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var nhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.Id);
                //if (nhomZalo == null)
                //{
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo");
                //}

                //return _mapper.Map<GetNhomZaloResponse>(nhomZalo);

                var repository = _unitOfWork.GetRepository<NhomZalo>();
                var userRepository = _unitOfWork.UserRepository;

                var nhomZaloById = await repository
                    .GetAllQueryable()
                    .Include(nz => nz.NhomZaloTasks)
                        .ThenInclude(nzt => nzt.Tasks) // không cần
                    .FirstOrDefaultAsync(nz => nz.Id == request.Id && !nz.IsDelete);

                if (nhomZaloById == null || nhomZaloById.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo.");
                }

                var response = _mapper.Map<GetNhomZaloResponse>(nhomZaloById);

                response.MoTaDuAnNhom = nhomZaloById.NhomZaloTasks.Select(nzt => nzt.Tasks.MoTa).ToList();

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
