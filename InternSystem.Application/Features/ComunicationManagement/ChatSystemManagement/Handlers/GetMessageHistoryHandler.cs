using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Models;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Handlers
{
    public class GetMessageHistoryHandler : IRequestHandler<GetMessageHistoryQuery, List<GetMessageHistoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageHistoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetMessageHistoryResponse>> Handle(GetMessageHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var messages = await _unitOfWork.MessageRepository
                              .GetMessagesAsync(request.IdSender, request.IdReceiver);

                var response = messages.Select(m => new GetMessageHistoryResponse
                {
                    IdSender = m.IdSender,
                    IdReceiver = m.IdReceiver,
                    MessageText = m.MessageText,
                    Timestamp = m.Timestamp
                }).ToList();

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
