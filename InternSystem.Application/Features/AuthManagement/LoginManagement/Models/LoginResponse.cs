namespace InternSystem.Application.Features.AuthManagement.LoginManagement.Models
{
    public class LoginResponse
    {
        public string VerificationToken { get; set; }
        public string ResetToken { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
