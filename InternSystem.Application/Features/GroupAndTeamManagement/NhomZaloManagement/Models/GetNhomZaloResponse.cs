using InternSystem.Domain.Entities;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models
{
    public class GetNhomZaloResponse 
    {
        public int Id { get; set; }
        public string TenNhom { get; set; }
        public string LinkNhom { get; set; }
        public List<string> MoTaDuAnNhom { get; set; }
        public bool IsNhomChung { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
    }
}
