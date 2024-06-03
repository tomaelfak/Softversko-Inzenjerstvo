using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Teams
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Team Team { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Team).SetValidator(new TeamValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            private readonly IUserAccesor _userAccessor;

            public Handler(DataContext context, IUserAccesor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                
                
                var participant = new TeamParticipant
                {
                    AppUser = user,
                    Team = request.Team,
                    IsCaptain = true
                };
                    
                request.Team.Participants.Add(participant);

                _context.Teams.Add(request.Team);



                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create team");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}