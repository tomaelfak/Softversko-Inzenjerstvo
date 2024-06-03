using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PrivateActivities
{
    public class UpdateParticipant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }

            
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccesor _userAccesor;
            public Handler(DataContext context, IUserAccesor userAccesor)
            {
                _userAccesor = userAccesor;
                _context = context;

            }


            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.PrivateActivities
                    .Include(x => x.Participants).ThenInclude(x => x.AppUser)
                    .Include(x => x.Team).ThenInclude(t => t.Participants).ThenInclude(tu => tu.AppUser) 
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null) return Result<Unit>.Failure("Activity not found"); ;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                if (user == null) return Result<Unit>.Failure("User not found"); ;

                // Check if user is part of the team
                if (!activity.Team.Participants.Any(tm => tm.AppUser.UserName == user.UserName))
                {
                    return Result<Unit>.Failure("User is not part of the team");
                }

                var HostUsername = activity.Participants.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                var participant = activity.Participants.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                if (participant != null && HostUsername == user.UserName)
                {
                    activity.IsCancelled = !activity.IsCancelled;
                }

                if (participant != null && HostUsername != user.UserName)
                {
                    activity.Participants.Remove(participant);
                    activity.NumOfParticipants -= 1;
                }

                if (participant == null && activity.NumOfParticipants < activity.MaxParticipants)
                {
                    participant = new ActivityParticipant
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Participants.Add(participant);
                    activity.NumOfParticipants += 1;
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating participants");
            }
        }
    }
}