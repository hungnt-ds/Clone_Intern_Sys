using System.Diagnostics;
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
    public class GetAllDuAnHandler : IRequestHandler<GetAllDuAnQuery, IEnumerable<GetAllDuAnResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllDuAnResponse>> Handle(GetAllDuAnQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var duAnRepository = _unitOfWork.GetRepository<DuAn>();
                var userRepository = _unitOfWork.UserRepository;

                var listDuAn = await duAnRepository
                    .GetAllQueryable()
                    .Include(da => da.Leader)
                    .Include(da => da.CongNgheDuAns)
                        .ThenInclude(cnda => cnda.CongNghe)
                    .Where(da => da.IsActive && !da.IsDelete)
                    .ToListAsync(cancellationToken);

                if (listDuAn == null || !listDuAn.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có dự án."
                    );
                }

                var response = _mapper.Map<IEnumerable<GetAllDuAnResponse>>(listDuAn);

                foreach (var duAnResponse in response)
                {
                    duAnResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                    duAnResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
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
