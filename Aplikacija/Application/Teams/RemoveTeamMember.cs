using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Teams
{
    public class RemoveTeamMember
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid TeamId { get; set; }
            public string Username { get; set; }
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
                var team = await _context.Teams.Include(x => x.Participants).FirstOrDefaultAsync(x => x.Id == request.TeamId);

                if (team == null) return Result<Unit>.Failure("Team not found");

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);

                if (user == null) return Result<Unit>.Failure("User not found");

                var member = team.Participants.FirstOrDefault(x => x.AppUserId == user.Id);

                if (member == null) return Result<Unit>.Failure("User is not a member of this team");

                if (member.IsCaptain) return Result<Unit>.Failure("You cannot remove yourself as captain");

                team.Participants.Remove(member);
                team.NumberOfTeammates--;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to remove team member");

                return Result<Unit>.Success(Unit.Value);
            }

        }
    }
}