namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models
{
    public class NhomZaloTaskReponse
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int NhomZaloId { get; set; }
        public string TrangThai { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}
