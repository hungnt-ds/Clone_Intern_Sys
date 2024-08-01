namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Models
{
    public class GetPagedViTrisResponse
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string? LinkNhomZalo { get; set; }
        public int? DuAnId { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public bool isActive { get; set; }
        public bool isDelete { get; set; }
    }
}
