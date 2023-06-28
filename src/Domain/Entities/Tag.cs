namespace Todo_App.Domain.Entities;
public class Tag: BaseAuditableEntity
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public TodoItem Item { get; set; } = null!;
}
