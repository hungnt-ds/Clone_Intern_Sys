using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Handlers
{
    public class GetInternStatsBySchoolIdHandler : IRequestHandler<GetInternStatsBySchoolIdQuery, List<GetInternStatsBySchoolIdResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInternStatsBySchoolIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetInternStatsBySchoolIdResponse>> Handle(GetInternStatsBySchoolIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var interns = await _unitOfWork.InternInfoRepository.GetInternStatsBySchoolIdAsync(request.SchoolId);
                if (interns == null || !interns.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông tin thực tập sinh với trường học được chọn");
                }
                return interns;
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
