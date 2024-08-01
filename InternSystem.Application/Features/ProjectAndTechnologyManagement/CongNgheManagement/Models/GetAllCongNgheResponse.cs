using InternSystem.Domain.Entities;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models
{
    public class GetAllCongNgheResponse
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public int IdViTri { get; set; }
        public string? TenViTri {  get; set; }
        public string UrlImage { get; set; }
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
