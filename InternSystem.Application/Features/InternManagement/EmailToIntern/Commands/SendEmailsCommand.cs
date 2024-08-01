using FluentValidation;
using InternSystem.Application.Common.Services.Interfaces;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Commands
{
    public class SendEmailsCommandValidator : AbstractValidator<SendEmailsCommand>
    {
        public SendEmailsCommandValidator(IEmailService emailService)
        {
            RuleFor(model => model.Subject)
            .NotEmpty().WithMessage("Subject không được để trống!");

            RuleFor(model => model.Body)
            .NotEmpty().WithMessage("Body không được để trống!");

            RuleFor(model => model.EmailType)
            .NotEmpty().WithMessage("Chưa chọn định dạng Email!")
            .Must(BeValidEmailType).WithMessage("Định dạng Email không đúng.");
        }

        private bool BeValidEmailType(string emailType)
        {
            var validEmailTypes = new List<string> { "Interview Date", "Interview Result", "Internship Time", "Internship Information" };
            return validEmailTypes.Contains(emailType);
        }
    }

    public class SendEmailsCommand : IRequest<bool>
    {
        public IEnumerable<string> SelectedEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailType { get; set; }

        public SendEmailsCommand(IEnumerable<string> selectedEmails, string subject, string body, string emailType)
        {
            SelectedEmails = selectedEmails;
            Subject = subject;
            Body = body;
            EmailType = emailType;
        }
    }
}
