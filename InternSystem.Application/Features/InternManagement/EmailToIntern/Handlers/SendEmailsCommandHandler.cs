using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.EmailToIntern.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Handlers
{
    public class SendEmailsCommandHandler : IRequestHandler<SendEmailsCommand, bool>
    {
        private readonly IEmailService _emailService;

        public SendEmailsCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> Handle(SendEmailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.SelectedEmails.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "No selected emails provided!");
                }

                if (string.IsNullOrEmpty(request.EmailType))
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Email type is required!");
                }

                string emailBody = request.Body;

                switch (request.EmailType)
                {
                    case "Interview Date":
                        emailBody = $"Interview Date Info: {request.Body}";
                        break;
                    case "Interview Result":
                        emailBody = $"Interview Result Info: {request.Body}";
                        break;
                    case "Internship Time":
                        emailBody = $"Internship Time Info: {request.Body}";
                        break;
                    case "Internship Information":
                        emailBody = $"Internship Information: {request.Body}";
                        break;
                    default:
                        throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, $"Invalid email type: {request.EmailType}");
                }

                var emailSent = await _emailService.SendEmailAsync(request.SelectedEmails, request.Subject, emailBody);

                if (!emailSent)
                {
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ErrorCode.Unknown, "Failed to send emails.");
                }

                return emailSent;
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
