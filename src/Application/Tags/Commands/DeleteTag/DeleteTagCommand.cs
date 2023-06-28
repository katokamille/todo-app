using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tags.Commands.CreateTags;
public record DeleteTagCommand(int Id) : IRequest;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context= context;  
    }

    public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags
            .Where(e => e.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(entity), request.Id);

        entity.Deleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}