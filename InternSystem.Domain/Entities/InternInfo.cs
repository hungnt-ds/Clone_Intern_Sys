using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{

    [Table("InternInfo")]
    public class InternInfo : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AspNetUser? AspNetUser { get; set; }

        [Required]
        [StringLength(255)]
        public string HoTen { get; set; }

        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        public bool GioiTinh { get; set; } // true: Nam, false: Nữ

        [StringLength(20)]
        public string MSSV { get; set; }

        [EmailAddress]
        public string EmailTruong { get; set; }

        [EmailAddress]
        public string EmailCaNhan { get; set; }

        [StringLength(10)]
        [Phone]
        public string Sdt { get; set; }

        [StringLength(10)]
        [Phone]
        public string SdtNguoiThan { get; set; }

        public string DiaChi { get; set; }

        public decimal GPA { get; set; }

        public string TrinhDoTiengAnh { get; set; }

        public string LinkFacebook { get; set; }

        public string LinkCv { get; set; }

        public string NganhHoc { get; set; }

        [Required]
        public string TrangThai { get; set; }

        public int Round { get; set; }

        [StringLength(255)]
        public string ViTriMongMuon { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public int IdTruong { get; set; }
        [ForeignKey("IdTruong")]
        public virtual TruongHoc TruongHoc { get; set; }

        public int? KyThucTapId { get; set; }
        [ForeignKey("KyThucTapId")]
        public virtual KyThucTap? KyThucTap { get; set; }

        public int? DuAnId { get; set; }
        [ForeignKey("DuAnId")]
        public virtual DuAn? DuAn { get; set; }

        public virtual ICollection<EmailUserStatus> EmailUserStatuses { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }

}
