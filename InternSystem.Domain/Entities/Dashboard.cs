using InternSystem.Domain.Entities.BaseEntities;

namespace InternSystem.Domain.Entities
{
    public class Dashboard : IBaseEntity
    {
        public int Id { get; set; }
        public int ReceivedCV { get; set; }
        public int Interviewed { get; set; }
        public int Passed { get; set; }
        public int Interning { get; set; }
        public int Interned { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
