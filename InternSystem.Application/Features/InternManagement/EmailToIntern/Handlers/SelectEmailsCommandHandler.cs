using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Handlers
{
    public class SelectEmailsCommandHandler : IRequestHandler<SelectEmailsCommand, IEnumerable<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SelectEmailsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<string>> Handle(SelectEmailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.InternInfoRepository.GetAllAsync() ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Intern not found !");

                var availableEmails = users.Select(user => user.EmailCaNhan).ToList();
                if (!availableEmails.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "No available email found found in intern!");
                }

                var selectedEmails = new List<string>();
                foreach (var index in request.Indices)
                {
                    if (index < 1 || index >= availableEmails.Count)
                    {
                        throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, $"Index {index} is out of bounds.");
                    }
                    var adjustedIndex = index - 1;
                    selectedEmails.Add(availableEmails[adjustedIndex]);
                }

                return selectedEmails;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lưu.");
            }
        }
    }
}
