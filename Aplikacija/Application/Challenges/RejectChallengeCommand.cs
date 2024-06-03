using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;

namespace Application.Challenges
{
    public class RejectChallengeCommand : IRequest<Result<Unit>>
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ChallengerTeamId { get; set; }
            public Guid ChallengeId { get; set; }
        }
    }
    
}