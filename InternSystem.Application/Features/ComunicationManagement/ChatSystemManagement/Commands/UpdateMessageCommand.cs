using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands
{
    public class UpdateMessageCommand : IRequest<bool>
    {
        public string MessageId { get; set; }
        public string NewMessageText { get; set; }

        public UpdateMessageCommand(string messageId, string newMessageText)
        {
            MessageId = messageId;
            NewMessageText = newMessageText;
        }
    }

    public class UpdateMessageCommandValidator : AbstractValidator<UpdateMessageCommand>
    {
        public UpdateMessageCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("Chưa chọn Message để Update!");
            RuleFor(x => x.NewMessageText)
                .NotEmpty().WithMessage("Tin nhắn mới không được để trống!");
        }
    }
}
