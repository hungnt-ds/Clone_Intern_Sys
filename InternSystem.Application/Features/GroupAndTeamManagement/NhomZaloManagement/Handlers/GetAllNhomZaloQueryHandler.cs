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
    public class GetAllNhomZaloQueryHandler : IRequestHandler<GetAllNhomZaloQuery, IEnumerable<GetNhomZaloResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllNhomZaloQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetNhomZaloResponse>> Handle(GetAllNhomZaloQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var nhomZaloRepository = _unitOfWork.GetRepository<NhomZalo>();
                var userRepository = _unitOfWork.UserRepository;

                var listNhomZalo = await nhomZaloRepository
                    .GetAllQueryable()
                    .Include(nz => nz.NhomZaloTasks)
                        .ThenInclude(nzt => nzt.Tasks)
                    .Where(nz => nz.IsActive && !nz.IsDelete)
                    .ToListAsync(cancellationToken);

                if(listNhomZalo == null || !listNhomZalo.Any())
                {
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không có nhóm Zalo.");
                }

                var response = _mapper.Map<IEnumerable<GetNhomZaloResponse>>(listNhomZalo);

                foreach(var nhomZaloResponse in response)
                {
                    nhomZaloResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(nhomZaloResponse.CreatedBy) ?? "Người dùng không xác định";
                    nhomZaloResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(nhomZaloResponse.LastUpdatedBy) ?? "Người dùng không xác định";
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
