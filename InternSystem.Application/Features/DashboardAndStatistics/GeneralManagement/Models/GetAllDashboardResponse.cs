namespace InternSystem.Application.Features.DashboardAndStatistics.GeneralManagement.Models
{
    public class GetAllDashboardResponse
    {
        public int ReceivedCV { get; set; }
        public int Interviewed { get; set; }
        public int Passed { get; set; }
        public int Interning { get; set; }
        public int Interned { get; set; }
        public string Errors { get; set; }
    }
}
