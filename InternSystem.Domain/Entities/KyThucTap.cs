using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("KyThucTap")]
    public class KyThucTap : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Ten { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        [Required]
        public int IdTruong { get; set; }
        [ForeignKey("IdTruong")]
        public TruongHoc TruongHoc { get; set; }
    }
}
