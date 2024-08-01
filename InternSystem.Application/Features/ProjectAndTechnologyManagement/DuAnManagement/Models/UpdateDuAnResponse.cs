namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models
{
    public class UpdateDuAnResponse
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string LeaderId { get; set; }
        public DateTimeOffset ThoiGianBatDau { get; set; }
        public DateTimeOffset ThoiGianKetThuc { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }


        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public string Errors { get; set; }
    }
}
