namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models
{
    public class GetCongNgheDuAnByIdResponse
    {
        public int Id { get; set; }
        public int IdCongNghe { get; set; }
        public int IdDuAn { get; set; }

        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }

        public string TenCongNghe { get; set; }
        public string TenDuAn { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string Errors { get; set; }
    }
}
