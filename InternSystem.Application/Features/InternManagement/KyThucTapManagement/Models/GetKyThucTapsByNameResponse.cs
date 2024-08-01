namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models
{
    public class GetKyThucTapsByNameResponse
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public int IdTruong { get; set; }

        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
