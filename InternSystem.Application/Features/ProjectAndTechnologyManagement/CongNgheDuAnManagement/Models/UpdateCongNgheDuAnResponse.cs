namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models
{
    public class UpdateCongNgheDuAnResponse
    {
        public int Id { get; set; }
        public int IdCongNghe { get; set; }
        public int IdDuAn { get; set; }
        public string LastUpdatedBy { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public string Errors { get; set; }
    }
}
