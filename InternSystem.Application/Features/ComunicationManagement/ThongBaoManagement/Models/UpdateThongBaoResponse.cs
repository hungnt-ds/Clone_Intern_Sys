using InternSystem.Domain.Entities;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models
{
    public class UpdateThongBaoResponse : ThongBao
    {
        public string? Errors { get; set; }
    }
}
