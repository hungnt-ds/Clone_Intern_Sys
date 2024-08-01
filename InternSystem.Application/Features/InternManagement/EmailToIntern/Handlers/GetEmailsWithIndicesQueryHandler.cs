using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Models;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Handlers
{
    public class GetEmailsWithIndicesQueryHandler : IRequestHandler<GetEmailsWithIndicesQuery, IEnumerable<EmailWithIndexResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmailsWithIndicesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmailWithIndexResponse>> Handle(GetEmailsWithIndicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.InternInfoRepository.GetAllAsync() ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Intern not found!");

                var activeUsers = users
                    .Where(u => u.IsActive == true && u.IsDelete == false)
                    .ToList();

                if (activeUsers == null || activeUsers.Count == 0)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy intern !");
                }

                var emailsWithIndices = users.Select((activeUsers, index) => new EmailWithIndexResponse { Index = index + 1, Email = activeUsers.EmailCaNhan });
                if (!emailsWithIndices.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy email!");
                }

                return emailsWithIndices;
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
