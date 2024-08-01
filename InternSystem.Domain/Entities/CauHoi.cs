using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("CauHoi")]
    public class CauHoi : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string NoiDung { get; set; }
        public ICollection<CauHoiCongNghe> CauHoiCongNghes { get; set; }
    }
}
