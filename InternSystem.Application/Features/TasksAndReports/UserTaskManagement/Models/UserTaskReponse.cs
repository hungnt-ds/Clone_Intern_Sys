namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models
{
    public class UserTaskReponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string TrangThai { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public string? DeletedBy { get; set; }

        public bool isActive { get; set; }
        public bool IsDelete { get; set; }

    }
}
