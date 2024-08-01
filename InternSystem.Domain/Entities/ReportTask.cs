using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("ReportTask")]
    public class ReportTask : BaseEntity
    {
        [Key]

        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AspNetUser User { get; set; }
        [Required]
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual Tasks Task { get; set; }
        [Required]
        public string MoTa { get; set; }

        [Required]
        public string NoiDungBaoCao { get; set; }

        public DateTime NgayBaoCao { get; set; }
        public string TrangThai { get; set; }
    }
}
