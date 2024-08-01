using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models
{
    public class GetPagedUserDuAnResponse
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int DuAnId { get; set; }
        public int IdViTri { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public bool isActive { get; set; }
        public bool isDelete { get; set; }
    }
}
