using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PrivateActivities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PrivateActivity PrivateActivity { get; set; }

            public Guid CourtId { get; set; }

            public Guid TeamId { get; set; }

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

            private readonly IUserAccesor _userAccesor;
            public Handler(DataContext context, IUserAccesor userAccesor)
            {
                _context = context;
                _userAccesor = userAccesor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
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

                    if (await Overlap(request.CourtId, /*request.PrivateActivity.Date,*/ request.StartTime, request.EndTime, cancellationToken))
                    {
                        return Result<Unit>.Failure("Court is already booked at that time!");
                    }

                    var timeSlot = new Domain.TimeSlot
                    {
                        CourtId = request.CourtId,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime,
                        Day = request.PrivateActivity.Date
                    };

                    var participant = new ActivityParticipant
                    {
                        AppUserId = user.Id,
                        Activity = request.PrivateActivity,
                        IsHost = true,
                    };

                    var team = await _context.Teams.FindAsync(request.TeamId);

                    bool teambool =  _context.Teams.Any(t => t.Id == request.TeamId);

                    if (!teambool)
                    {
                        return Result<Unit>.Failure("Team not found!");
                    }

                    request.PrivateActivity.TimeSlot = timeSlot;
                    request.PrivateActivity.Participants.Add(participant);
                    request.PrivateActivity.Court = court;
                    request.PrivateActivity.Price = court.PricePerHour * (request.EndTime - request.StartTime);
                    request.PrivateActivity.Team = team;

                    _context.TimeSlot.Add(timeSlot);

                    court.TimeSlots.Add(timeSlot);

                    _context.PrivateActivities.Add(request.PrivateActivity);

                    
                    var result = await _context.SaveChangesAsync() > 0;

                        if (!result)
                        {
                            return Result<Unit>.Failure("Failed to create private activity!");
                        }

                        return Result<Unit>.Success(Unit.Value);
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.InnerException);
                }

                return Result<Unit>.Failure("Failed to create private activity!");

                
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