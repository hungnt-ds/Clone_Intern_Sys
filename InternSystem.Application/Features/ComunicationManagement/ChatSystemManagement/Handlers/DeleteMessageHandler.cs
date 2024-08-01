using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Handlers
{
    public class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChatService _chatService;

        public DeleteMessageHandler(IUnitOfWork unitOfWork, IChatService chatService)
        {
            _unitOfWork = unitOfWork;
            _chatService = chatService;
        }

        public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var message = await _unitOfWork.MessageRepository.GetByIdAsync(request.MessageId);
                if (message == null)
                {
                    throw new ArgumentException("Message not found");
                }

                _unitOfWork.MessageRepository.Remove(message);
                await _unitOfWork.SaveChangeAsync();

                // Notify via SignalR
                await _chatService.NotifyMessageDeleted(message.IdReceiver, request.MessageId);
                await _chatService.NotifyMessageDeleted(message.IdSender, request.MessageId);

                return true;
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
