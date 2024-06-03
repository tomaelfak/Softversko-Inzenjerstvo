using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateParticipants
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var activity = await _context.Activities
                .Include(x => x.Participants).ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                if (user == null) return null;

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