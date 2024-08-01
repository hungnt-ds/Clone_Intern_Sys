using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("CauHoiCongNghe")]
    public class CauHoiCongNghe : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public int IdCongNghe { get; set; }
        [ForeignKey("IdCongNghe")]
        public virtual CongNghe CongNghe { get; set; }

        [Required]
        public int IdCauHoi { get; set; }
        [ForeignKey("IdCauHoi")]
        public virtual CauHoi CauHoi { get; set; }
    }
}
