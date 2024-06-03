using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courts
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            
            public Court Court { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Court).SetValidator(new CourtValidator());
            }
        }


        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccesor _userAccessor;
        
            private readonly DataContext _context;
            public Handler(DataContext context, IUserAccesor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                
            }
            
            

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                request.Court.Manager = user;

               _context.Courts.Add(request.Court);

               var result = await _context.SaveChangesAsync() > 0;

               if(!result) return Result<Unit>.Failure("Failed to create court");

               

                return Result<Unit>.Success(Unit.Value);
            }

           
        }
    }
}