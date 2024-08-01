using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("UserNhomZalo")]
    public class UserNhomZalo : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AspNetUser User { get; set; }

        public bool IsMentor { get; set; }
        public bool IsLeader { get; set; }

        public int? IdNhomZaloChung { get; set; }
        [ForeignKey("IdNhomZaloChung")]
        public virtual NhomZalo? NhomZaloChung { get; set; }

        public int? IdNhomZaloRieng { get; set; }
        [ForeignKey("IdNhomZaloRieng")]
        public virtual NhomZalo? NhomZaloRieng { get; set; }
    }
}
