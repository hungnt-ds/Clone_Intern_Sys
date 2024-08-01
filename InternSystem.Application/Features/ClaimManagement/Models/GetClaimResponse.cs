namespace InternSystem.Application.Features.ClaimManagement.Models
{
    public class GetClaimResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Errors { get; set; }
    }
}
