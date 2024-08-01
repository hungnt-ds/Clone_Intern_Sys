using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("CongNgheDuAn")]
    public class CongNgheDuAn : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public int IdCongNghe { get; set; }
        [ForeignKey("IdCongNghe")]
        public virtual CongNghe CongNghe { get; set; }

        [Required]
        public int IdDuAn { get; set; }
        [ForeignKey("IdDuAn")]
        public virtual DuAn DuAn { get; set; }

    }
}
