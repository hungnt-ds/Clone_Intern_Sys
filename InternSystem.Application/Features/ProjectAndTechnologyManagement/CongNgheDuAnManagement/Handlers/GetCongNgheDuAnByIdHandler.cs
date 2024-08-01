using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Handlers
{
    public class GetCongNgheDuAnByIdHandler : IRequestHandler<GetCongNgheDuAnByIdQuery, GetCongNgheDuAnByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCongNgheDuAnByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCongNgheDuAnByIdResponse> Handle(GetCongNgheDuAnByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var congNgheDuAnRepository = _unitOfWork.GetRepository<CongNgheDuAn>();
                var userRepository = _unitOfWork.UserRepository;

                var existingCNDA = await congNgheDuAnRepository.GetAllQueryable()
                    .Include(cnda => cnda.CongNghe)
                    .Include(cnda => cnda.DuAn)
                    .FirstOrDefaultAsync(cnda => cnda.Id == request.Id && !cnda.IsDelete, cancellationToken);

                if (existingCNDA == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy công nghệ"
                    );
                }

                var response = _mapper.Map<GetCongNgheDuAnByIdResponse>(existingCNDA);

                response.CreatedBy = await userRepository.GetUserNameByIdAsync(existingCNDA.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedBy = await userRepository.GetUserNameByIdAsync(existingCNDA.LastUpdatedBy) ?? "Người dùng không xác định";

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
