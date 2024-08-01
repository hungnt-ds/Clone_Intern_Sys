using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Domain.Entities
{
    public class AspNetUser : IdentityUser, IBaseEntity
    {
        public string HoVaTen { get; set; }
        public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? DeletedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public string LastUpdatedBy { get; set; } = string.Empty;
        public string? DeletedBy { get; set; }
        public string? ResetToken { get; set; }
        public DateTimeOffset? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTimeOffset? VerificationTokenExpires { get; set; }
        public bool IsConfirmed { get; set; }

        public int? InternInfoId { get; set; }
        [ForeignKey("InternInfoId")]
        public virtual InternInfo? InternInfo { get; set; }

        public string? TrangThaiThucTap { get; set; }
        public string? ImagePath { get; set; }
        public string? EmailCode { get; set; }

        public virtual ICollection<UserDuAn> UserDuAns { get; set; }
        public ICollection<UserViTri> UserViTris { get; set; }
        public ICollection<UserNhomZalo> UserNhomZalos { get; set; }
        public ICollection<EmailUserStatus> EmailUserStatuses { get; set; }
        public ICollection<ReportTask> reportTasks { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
    }
}