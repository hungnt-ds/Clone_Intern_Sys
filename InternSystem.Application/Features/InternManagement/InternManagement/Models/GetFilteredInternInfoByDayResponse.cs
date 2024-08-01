namespace InternSystem.Application.Features.InternManagement.InternManagement.Models
{
    public class GetFilteredInternInfoByDayResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string MSSV { get; set; }
        public string EmailTruong { get; set; }
        public string EmailCaNhan { get; set; }
        public string Sdt { get; set; }
        public string SdtNguoiThan { get; set; }
        public string DiaChi { get; set; }
        public decimal GPA { get; set; }
        public string TrinhDoTiengAnh { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkCv { get; set; }
        public string NganhHoc { get; set; }
        public string TrangThai { get; set; }
        public int Round { get; set; }
        public string ViTriMongMuon { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IdTruong { get; set; }
        public string TenTruong { get; set; }
        public int KyThucTapId { get; set; }
        public string TenKyThucTap {  get; set; }
        public int DuAnId { get; set; }
        public string TenDuAn { get; set; }
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
