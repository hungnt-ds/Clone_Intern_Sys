using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("LichPhongVan")]
    public class LichPhongVan : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string IdNguoiPhongVan { get; set; }
        [ForeignKey("IdNguoiPhongVan")]
        public virtual AspNetUser NguoiPhongVan { get; set; }

        [Required]
        public int IdNguoiDuocPhongVan { get; set; }
        [ForeignKey("IdNguoiDuocPhongVan")]
        public virtual InternInfo NguoiDuocPhongVan { get; set; }

        public DateTime ThoiGianPhongVan { get; set; }
        public string DiaDiemPhongVan { get; set; }
        public bool DaXacNhanMail { get; set; }
        public string SendMailResult { get; set; }
        public string InterviewForm { get; set; }
        public bool? TrangThai { get; set; }
        public string TimeDuration { get; set; }
        public string? KetQua { get; set; }
    }
}
