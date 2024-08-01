using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("ViTri")]
    public class ViTri : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Ten { get; set; }
        public string? LinkNhomZalo { get; set; }
        public int? DuAnId { get; set; }
        public ICollection<UserViTri> UserViTris { get; set; }
    }
}
