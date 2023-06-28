using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tags.Commands.CreateTags;
public class CreateTagCommand : IRequest<int>
{
    public int ItemId { get; set; }
    public string Name { get; set; }
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateTagCommandHandler(IApplicationDbContext context)
    {
        _context= context;  
    }

    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = new Tag
        {
            ItemId = request.ItemId,
            Name = request.Name
        };

        var tag = await _context.Tags
            .FirstOrDefaultAsync(e => e.ItemId == entity.ItemId &&
                                      e.Name.Equals(request.Name));
        
        if (tag is not null)
            throw new AlreadyExistsException(request.Name);

        _context.Tags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}