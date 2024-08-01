namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models
{
    public class GetLichPhongVanByTodayResponse
    {
        public int Id { get; set; }
        public string IdNguoiPhongVan { get; set; }
        public string TenNguoiPhongVan { get; set; }
        public int IdNguoiDuocPhongVan { get; set; }
        public string TenNguoiDuocPhongVan { get; set; }
        public DateTimeOffset ThoiGianPhongVan { get; set; }
        public string DiaDiemPhongVan { get; set; }
        public bool DaXacNhanMail { get; set; }
        public string SendMailResult { get; set; }
        public string InterviewForm { get; set; }
        public bool TrangThai { get; set; }
        public string TimeDuration { get; set; }
        public string KetQua { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
