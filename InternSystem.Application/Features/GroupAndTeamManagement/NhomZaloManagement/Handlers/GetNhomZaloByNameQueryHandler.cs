using AutoMapper;
using Azure;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.CustomExceptions;
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
    public class GetNhomZaloByNameQueryHandler : IRequestHandler<GetNhomZaloByNameQuery, GetNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNhomZaloByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetNhomZaloResponse> Handle(GetNhomZaloByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var nhomZalo = await _unitOfWork.NhomZaloRepository.GetNhomZalosByNameAsync(request.TenNhom);
                //if (nhomZalo == null)
                //{
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "không tìm thấy nhóm zalo");
                //}

                //return _mapper.Map<GetNhomZaloResponse>(nhomZalo);

                var repository = _unitOfWork.GetRepository<NhomZalo>();
                var userRepository = _unitOfWork.UserRepository;
                var nhomZaloQuery = repository.GetAllQueryable();

                //var nhomZaloByTen = await nhomZaloQuery
                //    .Include(nz => nz.NhomZaloTasks)
                //        .ThenInclude(nzt => nzt.Tasks)
                //    .Where(c => c.TenNhom == request.TenNhom && !c.IsDelete)
                //    .ToListAsync(cancellationToken);
                var nhomZaloByTen = await nhomZaloQuery
                    .Include(nz => nz.NhomZaloTasks)
                        .ThenInclude(nzt => nzt.Tasks)
                    .FirstOrDefaultAsync(nz => nz.TenNhom.Contains(request.TenNhom) && !nz.IsDelete);

                if (nhomZaloByTen == null || nhomZaloByTen.IsDelete)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo.");
                }

                var result = _mapper.Map<GetNhomZaloResponse>(nhomZaloByTen);

                result.CreatedByName = await userRepository.GetUserNameByIdAsync(result.CreatedBy) ?? "Người dùng không xác định";
                result.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(result.LastUpdatedBy) ?? "Người dùng không xác định";

                return result;
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
