using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Handlers
{
    public class GetAllCongNgheHandler : IRequestHandler<GetAllCongNgheQuery, IEnumerable<GetAllCongNgheResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllCongNgheResponse>> Handle(GetAllCongNgheQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var CongNghe = await _unitOfWork.CongNgheRepository.GetAllQueryable()
                    .Include(cn => cn.ViTri)
                    .Include(cn => cn.CauHoiCongNghes)
                    .Include(cn => cn.CongNgheDuAns)
                    .ToListAsync(cancellationToken)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy công nghệ.");

                var response = _mapper.Map<IEnumerable<GetAllCongNgheResponse>>(CongNghe);
                foreach (var congNgheResponse in response)
                {
                    congNgheResponse.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(congNgheResponse.CreatedBy!) ?? "Người dùng không xác định";
                    congNgheResponse.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(congNgheResponse.LastUpdatedBy!) ?? "Người dùng không xác định";
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
