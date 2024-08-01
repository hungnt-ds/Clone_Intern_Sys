namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Models
{
    public class GetPagedUserViTriResponse
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? TenUser { get; set; }
        public int IdViTri { get; set; }
        public string? TenViTri { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
