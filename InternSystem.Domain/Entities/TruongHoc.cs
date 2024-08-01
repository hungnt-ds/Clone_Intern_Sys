using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("TruongHoc")]
    public class TruongHoc : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Ten { get; set; }
        public int SoTuanThucTap { get; set; }
    }
}
