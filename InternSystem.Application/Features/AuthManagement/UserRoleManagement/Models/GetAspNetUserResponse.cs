using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Models
{
    public class GetAspNetUserResponse
    {
        public string? Id { get; set; }
        public string? HoVaTen { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? InternInfoId { get; set; }
    }
}
