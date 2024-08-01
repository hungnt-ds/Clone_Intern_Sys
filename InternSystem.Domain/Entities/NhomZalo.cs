using System.ComponentModel.DataAnnotations.Schema;
using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    [Table("NhomZalo")]
    public class NhomZalo : BaseEntity
    {
        public int Id { get; set; }
        public string TenNhom { get; set; }
        public string LinkNhom { get; set; }
        public bool IsNhomChung { get; set; }
        public virtual ICollection<NhomZaloTask> NhomZaloTasks { get; set; }
        public ICollection<UserNhomZalo> UserNhomZaloChungs { get; set; }
        public ICollection<UserNhomZalo> UserNhomZaloRiengs { get; set; }

    }
}
