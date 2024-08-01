using FluentValidation;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Commands
{
    public class SendMessageCommand : IRequest<bool>
    {
        public string IdSender { get; set; }
        public string IdReceiver { get; set; }
        public string MessageText { get; set; }

        public SendMessageCommand(string idSender, string idReceiver, string messageText)
        {
            IdSender = idSender;
            IdReceiver = idReceiver;
            MessageText = messageText;
        }
    }

    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(command => command.IdSender)
                .NotEmpty().WithMessage("id người gửi không được bỏ trống!");

            RuleFor(command => command.IdReceiver)
                .NotEmpty().WithMessage("Id người nhận không được bỏ trống!")
                .NotEqual(command => command.IdSender).WithMessage("Người gửi và người nhận không thể là cùng một người.");

            RuleFor(command => command.MessageText)
                .NotEmpty().WithMessage("Tin nhắn không được để trống!");
        }
    }
}
