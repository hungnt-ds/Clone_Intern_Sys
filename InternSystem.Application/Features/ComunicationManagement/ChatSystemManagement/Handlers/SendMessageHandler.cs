using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Handlers
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IChatService _chatService;
        private readonly ITimeService _timeService;
        public SendMessageHandler(IUnitOfWork unitOfWork, UserManager<AspNetUser> userManager, IChatService chatService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _chatService = chatService;
            _timeService = timeService;
        }

        public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate users exist
                var sender = await _userManager.FindByIdAsync(request.IdSender);
                if (sender == null)
                {
                    throw new ArgumentException("Sender not found");
                }

                var receiver = await _userManager.FindByIdAsync(request.IdReceiver);
                if (receiver == null)
                {
                    throw new ArgumentException("Receiver not found");
                }

                // Validate sender and receiver are not the same
                if (request.IdSender == request.IdReceiver)
                {
                    throw new ArgumentException("Sender and receiver cannot be the same");
                }

                var message = new Domain.Entities.Message
                {
                    Id = Guid.NewGuid().ToString(),
                    IdSender = request.IdSender,
                    IdReceiver = request.IdReceiver,
                    MessageText = request.MessageText,
                    Timestamp = _timeService.SystemTimeNow.DateTime,

                    CreatedBy = request.IdSender,
                    LastUpdatedBy = request.IdSender,
                    CreatedTime = _timeService.SystemTimeNow,
                    LastUpdatedTime = _timeService.SystemTimeNow,
                    IsActive = true,
                    IsDelete = false
                };



                await _unitOfWork.MessageRepository.AddAsync(message);
                await _unitOfWork.SaveChangeAsync();

                // Notify the recipient via SignalR
                //await _hubContext.Clients.User(request.IdReceiver).SendAsync("ReceiveMessage", message.IdSender, message.MessageText);
                await _chatService.NotifyMessageReceived(request.IdReceiver, message.IdSender, message.MessageText);
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
