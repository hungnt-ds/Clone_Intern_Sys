namespace InternSystem.Application.Features.InternManagement.InternManagement.Models
{
    public class GetInternInfoResponse
    {
        public int Id { get; set; }
        public string? TrangThai { get; set; }
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool? GioiTinh { get; set; }
        public string? MSSV { get; set; }
        public string? EmailTruong { get; set; }
        public string? EmailCaNhan { get; set; }
        public string? Sdt { get; set; }
        public string? DiaChi { get; set; }
        public string? TruongHoc { get; set; }
        public string? KyThucTap { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
