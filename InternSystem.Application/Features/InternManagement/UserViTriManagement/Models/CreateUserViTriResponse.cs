namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Models
{
    public class CreateUserViTriResponse
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public int IdViTri { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }


        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public string? Errors { get; set; }
    }
}
