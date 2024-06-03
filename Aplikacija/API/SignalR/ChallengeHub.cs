using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Challenges;
using Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Persistence;

namespace API.SignalR
{
    public class ChallengeHub : Hub
    {
        private readonly IMediator _mediator;
     

        public ChallengeHub(IMediator mediator)
        {
            _mediator = mediator;
            
        }
        
        public async Task SendChallenge(Guid challengerTeamId, Guid challengedTeamId, Guid courtId, TimeSlot timeSlot )
        {
            var challenge = await _mediator.Send(new SendChallengeCommand.Command
            {
                ChallengerTeamId = challengerTeamId,
                ChallengedTeamId = challengedTeamId,
                CourtId = courtId,
                TimeSlot = timeSlot
            });

            await Clients.Group(challengedTeamId.ToString()).SendAsync("ReceiveChallenge", challenge);
            
            
        }

        public async Task AcceptChallenge(Guid challengerTeamId, Guid challengeId)
        {
            await _mediator.Send(new AcceptChallengeCommand.Command
            {
                ChallengerTeamId = challengerTeamId,
                ChallengeId = challengeId
            });

            await Clients.Group(challengerTeamId.ToString()).SendAsync("ChallengeAccepted");
        }

        public async Task RejectChallenge(Guid challengerTeamId, Guid challengeId)
        {
            await _mediator.Send(new RejectChallengeCommand.Command
            {
                ChallengerTeamId = challengerTeamId,
                ChallengeId = challengeId
            });

            await Clients.Group(challengerTeamId.ToString()).SendAsync("ChallengeRejected");
        }
    }
}