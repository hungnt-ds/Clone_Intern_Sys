using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Handlers
{
    public class UpdateMessageHandler : IRequestHandler<UpdateMessageCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChatService _chatService;
        private readonly ITimeService _timeService;
        private readonly IUserContextService _userContextService;

        public UpdateMessageHandler(IUnitOfWork unitOfWork, IChatService chatService, ITimeService timeService, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _chatService = chatService;
            _timeService = timeService;
            _userContextService = userContextService;
        }

        public async Task<bool> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                string currentUserId = _userContextService.GetCurrentUserId();
                var message = await _unitOfWork.MessageRepository.GetByIdAsync(request.MessageId);
                if (message == null)
                {
                    throw new ArgumentException("Message not found");
                }

                message.MessageText = request.NewMessageText;

                message.LastUpdatedBy = currentUserId;
                message.LastUpdatedTime = _timeService.SystemTimeNow;
                _unitOfWork.MessageRepository.UpdateMessageAsync(message);
                await _unitOfWork.SaveChangeAsync();


                //// Notify via SignalR
                //await _hubContext.Clients.User(message.IdReceiver).SendAsync("UpdateMessage", request.MessageId, request.NewMessageText);
                //await _hubContext.Clients.User(message.IdSender).SendAsync("UpdateMessage", request.MessageId, request.NewMessageText);
                //return true;

                // Notify via SignalR
                await _chatService.NotifyMessageUpdated(message.IdReceiver, request.MessageId, request.NewMessageText);
                await _chatService.NotifyMessageUpdated(message.IdSender, request.MessageId, request.NewMessageText);

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
