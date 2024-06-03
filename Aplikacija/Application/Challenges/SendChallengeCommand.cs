using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Challenges
{
    public class SendChallengeCommand : IRequest<Result<Challenge>>
    {
        public class Command :  IRequest<Result<Challenge>>
        {
            public Guid ChallengerTeamId { get; set; }
            public Guid ChallengedTeamId { get; set; }
            public Guid CourtId { get; set; }
            public Domain.TimeSlot TimeSlot { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Challenge>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<Challenge>> Handle(Command request, CancellationToken cancellationToken)
            {

                var court = await _context.Courts.FindAsync(request.CourtId);

                if (court == null)
                {
                    return Result<Challenge>.Failure("Court not found!");
                }

                if (!InWorkingTime(court, request.TimeSlot.StartTime, request.TimeSlot.EndTime))
                {
                    return Result<Challenge>.Failure("Court is not working at that time!");
                }

                if (await Overlap(request.CourtId, request.TimeSlot.Day, request.TimeSlot.StartTime, request.TimeSlot.EndTime, cancellationToken))
                {
                    return Result<Challenge>.Failure("Court is already booked at that time!");
                }

                var challenge = new Challenge
                {
                    ChallengerTeamId = request.ChallengerTeamId,
                    ChallengedTeamId = request.ChallengedTeamId,
                    CourtId = request.CourtId,
                    TimeSlot = request.TimeSlot,
                    Status = ChallengeStatus.Pending
                };

                var timeslot = new Domain.TimeSlot
                {
                    CourtId = request.CourtId,
                    Day = request.TimeSlot.Day,
                    StartTime = request.TimeSlot.StartTime,
                    EndTime = request.TimeSlot.EndTime
                };

                _context.TimeSlot.Add(timeslot);

                _context.Challenges.Add(challenge);
                var success = await _context.SaveChangesAsync() > 0;

                if (!success) return Result<Challenge>.Failure("Failed to send challenge");
                
                return Result<Challenge>.Success(challenge);
            }

            public async Task<bool> Overlap(Guid courtId, DateTime day, int startTime, int endTime, CancellationToken cancellationToken)
            {
                return await _context.TimeSlot.AnyAsync(x => x.CourtId == courtId && x.Day == day && x.StartTime < endTime && x.EndTime > startTime, cancellationToken);

            }

            public bool InWorkingTime(Court court, int startTime, int endTime)
            {
                return court.StartTime <= startTime && court.EndTime >= endTime;
            }
        }
    }
}