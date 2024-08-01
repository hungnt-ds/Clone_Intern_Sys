using InternSystem.Domain.Entities;
using System.Text.Json.Serialization;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Models
{
    public class GetAllUserResponse 
    {
        public string Id { get; set; }
        public string HoVaTen { get; set; }
        public DateTimeOffset CreatedTime { get; set; } 
        public DateTimeOffset? DeletedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; } 
        public bool IsDelete { get; set; } 
        public string CreatedBy { get; set; } 
        public string LastUpdatedBy { get; set; } 
        public string LastUpdatedByName { get; set; } 
        public string CreatedByName { get; set; } 
        public bool IsConfirmed { get; set; }

        public string? TrangThaiThucTap { get; set; }
        public string? ImagePath { get; set; }

        public List<string> ListDuAn { get; set; }
        public List<string> ListViTri { get; set; }
        public List<string> ListNhomZalo { get; set; }
    }
}
