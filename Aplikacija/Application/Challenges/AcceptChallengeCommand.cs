using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Challenges
{
    public class AcceptChallengeCommand : IRequest<Result<Unit>>
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ChallengerTeamId { get; set; }
            public Guid ChallengeId { get; set; }
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
                var challenge = await _context.Challenges.FindAsync(request.ChallengeId);

                if (challenge == null)
                {
                    return Result<Unit>.Failure("Challenge not found!");
                }

                challenge.Status = ChallengeStatus.Accepted;

                var success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Unit>.Failure("Failed to accept challenge");

                return Result<Unit>.Success(Unit.Value);
                
            }
        }
    }
   
}