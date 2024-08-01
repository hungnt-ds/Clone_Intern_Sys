namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Models
{
    public class UpdateViTriResponse
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public string? LinkNhomZalo { get; set; }
        public int? DuAnId { get; set; }
        public string Errors { get; internal set; }
    }
}
