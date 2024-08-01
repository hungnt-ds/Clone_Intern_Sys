using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InternSystem.Domain.Entities;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models
{
    public class GetUserNhomZaloResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TenNguoiDung { get; set; }
        public bool IsMentor { get; set; }
        public bool IsLeader { get; set; }
        public int? IdNhomZaloChung { get; set; }
        public string? TenNhomZaloChung { get; set; }
        public int? IdNhomZaloRieng { get; set; }
        public string? TenNhomZaloRieng { get; set; }
    }
}
