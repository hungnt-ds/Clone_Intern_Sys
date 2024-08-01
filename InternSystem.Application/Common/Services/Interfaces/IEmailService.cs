namespace InternSystem.Application.Common.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(IEnumerable<string> toList, string subject, string body);
    }
}
