using Todo_App.Application.Common.Mappings;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tags.Queries.GetTags;
public class TagDto : IMapFrom<Tag>
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string Name { get; set; }
}
