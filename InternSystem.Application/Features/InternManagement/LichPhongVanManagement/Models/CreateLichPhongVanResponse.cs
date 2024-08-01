namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models
{
    public class CreateLichPhongVanResponse
    {
        public int Id { get; set; }
        public string IdNguoiPhongVan { get; set; }
        public int IdNguoiDuocPhongVan { get; set; }
        public DateTime ThoiGianPhongVan { get; set; }
        public string DiaDiemPhongVan { get; set; }
        public bool DaXacNhanMail { get; set; }
        public string? SendMailResult { get; set; }
        public string? InterviewForm { get; set; }
        public bool TrangThai { get; set; }
        public string? TimeDuration { get; set; }
        public string? KetQua { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string? Errors { get; set; }
    }

}
