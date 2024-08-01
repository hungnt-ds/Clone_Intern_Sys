namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models
{
    public class GetCauHoiCongNgheByIdResponse
    {
        public int Id { get; set; }

        public int IdCongNghe { get; set; }
        public string TenCongNghe { get; set; }
        public int IdCauHoi { get; set; }
        public string NoiDungcauHoi { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public string Errors { get; set; }
    }
}
