using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.TimeSlot;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courts
{
    public class ScheduledSlots
    {
        public class Query : IRequest<Result<List<TimeSlotDto>>>
        {
            public Guid Id { get; set; }
            public DateTime Date { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<TimeSlotDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;

            }

            public async Task<Result<List<TimeSlotDto>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var court = await _context.Courts.Include(c => c.TimeSlots).SingleOrDefaultAsync(c => c.Id == request.Id);

                if (court == null) return Result<List<TimeSlotDto>>.Failure("Court not found");

                var timeslots = court.TimeSlots.Where(x => x.Day == request.Date).ToList();


                var timeslotsDtos = _mapper.Map<List<TimeSlotDto>>(timeslots);

                return Result<List<TimeSlotDto>>.Success(timeslotsDtos);
            }
        }
    }
}