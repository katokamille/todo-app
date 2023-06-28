namespace Todo_App.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public bool Deleted { get; set; }
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
