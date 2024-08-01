namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Models
{
    public class GetPagedTasksResponse
    {
        public int Id { get; set; }
        public int DuAnId { get; set; }
        public string TenDuAn { get; set; }
        public string MoTa { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayGiao { get; set; }
        public DateTime HanHoanThanh { get; set; }
        public bool HoanThanh { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
