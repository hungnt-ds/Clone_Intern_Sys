using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("Message")]
    public class Message : IBaseEntity
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }
        [Required]
        public string IdSender { get; set; }
        [ForeignKey("IdSender")]
        public virtual AspNetUser Sender { get; set; }
        [Required]
        public string IdReceiver { get; set; }
        [ForeignKey("IdReceiver")]
        public virtual AspNetUser Receiver { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public string MessageText { get; set; }
        public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? DeletedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}
