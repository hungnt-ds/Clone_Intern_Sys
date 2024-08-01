namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models
{
    public class CreateNhomZaloResponse
    {
        public int Id { get; set; }
        public string? TenNhom { get; set; }
        public string? LinkNhom { get; set; }
        public DateTimeOffset? CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
    }
}
