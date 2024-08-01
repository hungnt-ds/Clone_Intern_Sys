namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models
{
    public class GetAllPhongVanResponse
    {
        public int Id { get; set; }
        public string CauTraLoi { get; set; }
        public decimal Rank { get; set; }
        public string NguoiCham { get; set; }
        public DateTime RankDate { get; set; }
        public int IdCauHoiCongNghe { get; set; }
        public string NoiDungCauHoi { get; set; }
        public string TenCongNghe { get; set; }
        public int IdLichPhongVan { get; set; }
        public string TenNguoiPhongVan { get; set; }
        public DateTime ThoiGianPhongVan { get; set; }
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
