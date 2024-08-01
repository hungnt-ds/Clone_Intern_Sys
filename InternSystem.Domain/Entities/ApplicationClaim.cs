using System.ComponentModel.DataAnnotations;
using InternSystem.Domain.Entities.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Domain.Entities
{
    public class ApplicationClaim : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public ICollection<IdentityRoleClaim<string>> RoleClaims { get; set; }
    }
}
