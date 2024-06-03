using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PrivateActivities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Domain.PrivateActivity PrivateActivity { get; set; }

            public int StartTime { get; set; }

            public int EndTime { get; set; }


        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PrivateActivity).SetValidator(new PrivateActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.PrivateActivities.FindAsync(request.PrivateActivity.Id);

                if (activity == null) return Result<Unit>.Failure($"Activity not found");

                var court = await _context.Courts.FindAsync(activity.CourtId);
                if (court == null)
                {

                    return Result<Unit>.Failure($"Court not found");
                }

                if (!InWorkingTime(court, request.StartTime, request.EndTime))
                {
                    return Result<Unit>.Failure("Court is not working at that time!");
                }

                if (await Overlap(court.Id, activity.TimeSlotId, request.StartTime, request.EndTime, cancellationToken))
                {
                    return Result<Unit>.Failure("Time slot is already taken!");
                }

                if(request.PrivateActivity.MaxParticipants < activity.Participants.Count)
                {
                    return Result<Unit>.Failure("Max participants cannot be less than current participants!");
                }

                var timeslot = await _context.TimeSlot.FindAsync(activity.TimeSlotId);

                timeslot.StartTime = request.StartTime;
                timeslot.EndTime = request.EndTime;


                _mapper.Map(request.PrivateActivity, activity);

                _context.TimeSlot.Update(activity.TimeSlot);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update activity");

                return Result<Unit>.Success(Unit.Value);
            }

            public async Task<bool> Overlap(Guid courtId, Guid timeSlotId, int startTime, int endTime, CancellationToken cancellationToken)
            {
                return await _context.TimeSlot
                .Where(x => x.CourtId == courtId && x.Id != timeSlotId)
                .AnyAsync(x => x.CourtId == courtId /*&& x.Day == day*/ && x.StartTime < endTime && x.EndTime > startTime, cancellationToken);

            }

            public bool InWorkingTime(Court court, int startTime, int endTime)
            {
                return court.StartTime <= startTime && court.EndTime >= endTime;
            }
        }
    }
}