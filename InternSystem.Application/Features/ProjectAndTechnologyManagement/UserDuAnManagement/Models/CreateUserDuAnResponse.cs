namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models
{
    public class CreateUserDuAnResponse
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int DuAnId { get; set; }
        public int IdViTri { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }


        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public string? Errors { get; set; }
    }
}
