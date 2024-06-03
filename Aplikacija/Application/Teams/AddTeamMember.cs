using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Teams
{
    public class AddTeamMember
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid TeamId { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccesor _userAccesor;

            private readonly DataContext _context;

            public Handler(DataContext context, IUserAccesor userAccesor)
            {
                _userAccesor = userAccesor;
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var team = await _context.Teams.FindAsync(request.TeamId);

                if (team == null) return Result<Unit>.Failure("Failed to find team"); ;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);

                if (user == null) return Result<Unit>.Failure("Failed to find member with specified username"); 

                var teamMember = team.Participants.FirstOrDefault(x => x.AppUser.UserName == request.UserName);



                if (teamMember != null) return Result<Unit>.Failure("Failed to add team member, already teammate.");

                teamMember = new TeamParticipant
                {
                    Team = team,
                    AppUser = user,
                    IsCaptain = false
                };
                team.NumberOfTeammates++;
                team.Participants.Add(teamMember);



                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to add team member");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}