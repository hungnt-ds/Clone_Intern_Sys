using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("PhongVan")]
    public class PhongVan : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string CauTraLoi { get; set; }
        public decimal Rank { get; set; }

        [Required]
        public string NguoiCham { get; set; }


        public DateTime RankDate { get; set; }

        [Required]
        public int IdCauHoiCongNghe { get; set; }
        [ForeignKey("IdCauHoiCongNghe")]
        public virtual CauHoiCongNghe CauHoiCongNghe { get; set; }

        [Required]
        public int IdLichPhongVan { get; set; }
        [ForeignKey("IdLichPhongVan")]
        public virtual LichPhongVan LichPhongVan { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}
