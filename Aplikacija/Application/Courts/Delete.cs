using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Courts
{
    public class Delete
    {
            public class Command : IRequest<Result<Unit>>
            {
                public Guid id {get; set;}
            }
        

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
                
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var court = await _context.Courts.FindAsync(request.id);

                if(court == null) return null;

                _context.Remove(court);

                var result = await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Failure("Failed to delete court");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}