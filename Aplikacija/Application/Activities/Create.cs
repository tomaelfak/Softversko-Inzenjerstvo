using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>> // Command ne vraca nista
        {
            public Activity Activity { get; set; }
            public Court Court { get; set; }
            public Guid CourtId { get; set; }

            public int StartTime { get; set; }

            public int EndTime { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccesor _userAccesor;
            public Handler(DataContext context, IUserAccesor userAccesor)
            {
                _context = context;
                _userAccesor = userAccesor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                var court = await _context.Courts.FindAsync(request.CourtId);


                if (court == null)
                {
                    return Result<Unit>.Failure("Court not found!");
                }

                if (!InWorkingTime(court, request.StartTime, request.EndTime))
                {
                    return Result<Unit>.Failure("Court is not working at that time!");
                }

                if (await Overlap(request.CourtId, /*request.Activity.Date,*/ request.StartTime, request.EndTime, cancellationToken))
                {
                    return Result<Unit>.Failure("Time slot is already taken!");
                }

                var timeSlot = new Domain.TimeSlot
                {
                    CourtId = request.CourtId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Day = request.Activity.Date
                };

                var participant = new ActivityParticipant
                {
                    AppUserId = user.Id,
                    Activity = request.Activity,
                    IsHost = true,
                };

                request.Activity.TimeSlot = timeSlot;
                request.Activity.Participants.Add(participant);
                request.Activity.Court = court;
                request.Activity.Price = court.PricePerHour * (request.EndTime - request.StartTime);

                _context.TimeSlot.Add(timeSlot);

                court.TimeSlots.Add(timeSlot);

                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create activity");

                return Result<Unit>.Success(Unit.Value);


            }

            public async Task<bool> Overlap(Guid courtId, /*DateTime day,*/ int startTime, int endTime, CancellationToken cancellationToken)
            {
                return await _context.TimeSlot.AnyAsync(x => x.CourtId == courtId /*&& x.Day == day*/ && x.StartTime < endTime && x.EndTime > startTime, cancellationToken);

            }

            public bool InWorkingTime(Court court, int startTime, int endTime)
            {
                return court.StartTime <= startTime && court.EndTime >= endTime;
            }
        }
    }
}