namespace InternSystem.Domain.Entities.BaseEntities;

public class BaseEntity : IBaseEntity
{
    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    public string CreatedBy { get; set; }
    public string LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDelete { get; set; } = false;
}