namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models
{
    public class GetAllThongBaoResponse
    {
        public int Id { get; set; }
        public string IdNguoiNhan { get; set; }
        public string TenNguoiNhan { get; set; }
        public string IdNguoiGui { get; set; }
        public string TenNguoiGui { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string TinhTrang { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
