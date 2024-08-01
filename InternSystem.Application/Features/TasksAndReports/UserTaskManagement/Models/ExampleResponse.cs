namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models
{
    public class ExampleResponse
    {
        public int NhomZaloId { get; set; }
        public string Leader { get; set; }
        public string Mentor { get; set; }
        public TaskDetails Task { get; set; }
    }

    public class TaskDetails
    {
        public int DuanId { get; set; }
        public string Leader { get; set; }
    }
}

