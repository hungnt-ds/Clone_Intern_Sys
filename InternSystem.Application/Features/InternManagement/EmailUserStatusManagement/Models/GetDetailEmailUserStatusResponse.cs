namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models
{
    public class GetDetailEmailUserStatusResponse
    {
        public int Id { get; set; }
        public string IdNguoiGui { get; set; }
        public int IdNguoiNhan { get; set; }
        public string EmailLoai1 { get; set; }
        public string EmailLoai2 { get; set; }
        public string EmailLoai3 { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string? Errors { get; set; }
    }
}
