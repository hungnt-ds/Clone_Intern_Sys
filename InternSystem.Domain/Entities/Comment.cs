using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int IdNguoiDuocComment { get; set; }
        [ForeignKey("IdNguoiDuocComment")]
        public virtual InternInfo NguoiDuocComment { get; set; }

        [Required]
        public string IdNguoiComment { get; set; }
        [ForeignKey("IdNguoiComment")]
        public virtual AspNetUser NguoiComment { get; set; }
    }
}
