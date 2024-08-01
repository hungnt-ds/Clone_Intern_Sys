using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("ThongBao")]
    public class ThongBao : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdNguoiNhan { get; set; }
        [ForeignKey("IdNguoiNhan")]
        public virtual AspNetUser NguoiNhan { get; set; }

        [Required]
        public string IdNguoiGui { get; set; }
        [ForeignKey("IdNguoiGui")]
        public virtual AspNetUser NguoiGui { get; set; }

        [Required]
        public string TieuDe { get; set; }

        [Required]
        public string NoiDung { get; set; }
        public string TinhTrang { get; set; }
    }
}
