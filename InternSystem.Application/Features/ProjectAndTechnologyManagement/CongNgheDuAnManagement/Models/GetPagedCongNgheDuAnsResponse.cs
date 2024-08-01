namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models
{
    public class GetPagedCongNgheDuAnsResponse
    {
        public int Id { get; set; }
        public int IdCongNghe { get; set; }
        public string TenCongNghe { get; set; }
        public int IdDuAn { get; set; }
        public string TenDuAn { get; set; }
    }
}
