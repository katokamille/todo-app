using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.Tags.Queries.SumaryTags;
public record SummaryTagsQuery: IRequest<List<SummaryTagDto>>;

public class SummaryTagsQueryHandler : IRequestHandler<SummaryTagsQuery, List<SummaryTagDto>>
{
    private readonly IApplicationDbContext _context;

    public SummaryTagsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<SummaryTagDto>> Handle(SummaryTagsQuery request, CancellationToken cancellationToken)
    {
        return (from tag in _context.Tags
                group tag by tag.Name into grp
                select new SummaryTagDto
                {
                    Name = grp.Key,
                    Count = grp.Count()
                }).ToListAsync();
    }
}
