using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

public class GetFilteredInternInfoQueryHandler : IRequestHandler<GetFilteredInternInfoQuery, IEnumerable<InternInfo>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFilteredInternInfoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<InternInfo>> Handle(GetFilteredInternInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var interns = await _unitOfWork.InternInfoRepository.GetFilterdInternInfosAsync(request.SchoolId, request.StartDate, request.EndDate);
            if (interns == null || !interns.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông tin thực tập sinh theo ngày được chọn");
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