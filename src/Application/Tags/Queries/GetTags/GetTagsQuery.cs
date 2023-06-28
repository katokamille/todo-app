using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.Common.Mappings;

namespace Todo_App.Application.Tags.Queries.GetTags;
public record GetTagsQuery : IRequest<List<TagDto>>
{
    public int ItemId { get; init; }
}

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<TagDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return _context.Tags
            .Where(x => x.ItemId == request.ItemId)
            .OrderBy(x => x.Name)
            .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}