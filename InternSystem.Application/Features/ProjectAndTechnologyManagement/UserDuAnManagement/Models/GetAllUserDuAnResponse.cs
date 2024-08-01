using System.Runtime.CompilerServices;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models
{
    public class GetAllUserDuAnResponse
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int DuAnId { get; set; }
        public int IdViTri { get; set; }
        public string? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        
        public bool isActive { get; set; }
        public bool isDelete { get; set; }
    }
}
