using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.DashboardAndStatistics.InternManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.User.Handlers
{
    namespace InternSystem.Application.Features.User.Handlers
    {
        public class GetTotalInternStudentsQueryHandler : IRequestHandler<GetTotalInternStudentsQuery, int>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetTotalInternStudentsQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<int> Handle(GetTotalInternStudentsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var totalInternStudents = await _unitOfWork.InternInfoRepository.GetTotalInternStudentsByDateRangeAsync(request.StartDate, request.EndDate);
                    if (totalInternStudents == 0)
                    {
                        throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không có thực tập sinh nào trong khoảng thời gian này");
                    }

                    return totalInternStudents;
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
}
