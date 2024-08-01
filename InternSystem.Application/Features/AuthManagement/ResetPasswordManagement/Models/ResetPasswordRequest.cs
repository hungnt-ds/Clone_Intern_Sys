namespace InternSystem.Application.Features.AuthManagement.ResetPasswordManagement.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        //public string Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
