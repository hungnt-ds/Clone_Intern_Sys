namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models
{
    public class UpdateCauHoiCongNgheResponse
    {
        public int Id { get; set; }

        public int IdCongNghe { get; set; }
        public int IdCauHoi { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public string Errors { get; set; }
    }
}
