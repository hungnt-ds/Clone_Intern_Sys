using System.Net;
using System.Net.Mail;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Domain.BaseException;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InternSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _senderEmail = configuration["EmailSettings:Sender"]
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Email Sender is not configured.");
            var password = configuration["EmailSettings:Password"];
            var host = configuration["EmailSettings:Host"];
            var port = int.Parse(configuration["EmailSettings:Port"]
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Email port is not configured."));

            _smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderEmail, password)
            };

            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(IEnumerable<string> toList, string subject, string body)
        {
            try
            {
                foreach (var to in toList)
                {
                    var mailMessage = new MailMessage(_senderEmail, to, subject, body);
                    await _smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email.");
                return false;
            }

            return true;
        }
    }

    //public class EmailService : IEmailService
    //{
    //    private readonly IConfiguration _configuration;

    //    private static List<string> availableEmails = new List<string>
    //    {
    //        "truongdaivy57@gmail.com",
    //        "eraishopping57@gmail.com"
    //    };

    //    public List<string> GetAvailableEmails()
    //    {
    //        return availableEmails;
    //    }

    //    public EmailService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public bool SendEmail(IEnumerable<string> toList, string subject, string body)
    //    {
    //        try
    //        {
    //            var sender = _configuration["EmailSettings:Sender"];
    //            var password = _configuration["EmailSettings:Password"];
    //            var host = _configuration["EmailSettings:Host"];
    //            var port = int.Parse(_configuration["EmailSettings:Port"]);

    //            using (var client = new SmtpClient(host, port))
    //            {
    //                client.EnableSsl = true;
    //                NetworkCredential credential = new NetworkCredential(sender, password);
    //                client.UseDefaultCredentials = false;
    //                client.Credentials = credential;

    //                foreach (var to in toList)
    //                {
    //                    MailMessage msg = new MailMessage(sender, to, subject, body);
    //                    client.Send(msg);
    //                }
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }
    //}
}
