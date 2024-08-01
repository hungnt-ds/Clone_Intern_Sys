namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Models
{
    public class SendEmailsRequest
    {
        public List<int> Indices { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailType { get; set; }
    }
}
