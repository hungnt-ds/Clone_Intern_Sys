using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Handlers
{
    public class GetDuAnByIdHandler : IRequestHandler<GetDuAnByIdQuery, GetDuAnByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDuAnByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetDuAnByIdResponse> Handle(GetDuAnByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<DuAn>();
                var userRepository = _unitOfWork.UserRepository;

                var duAnById = await repository
                    .GetAllQueryable()
                    .Include(da => da.Leader)
                    .Include(d => d.CongNgheDuAns)  
                        .ThenInclude(cnda => cnda.CongNghe)
                    .FirstOrDefaultAsync(da => da.Id == request.Id && !da.IsDelete);

                if (duAnById == null || duAnById.IsDelete)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Dự Án");

                var response = _mapper.Map<GetDuAnByIdResponse>(duAnById);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";
                
                // response.TenCongNghe = duAnById.CongNgheDuAns.Select(cnda => cnda.CongNghe.Ten).ToList();

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
