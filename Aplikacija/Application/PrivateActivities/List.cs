using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PrivateActivities
{
    public class List
    {
        public class Query : IRequest<Result<List<PrivateActivityDto>>> 
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<PrivateActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<Result<List<PrivateActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.PrivateActivities
                    .Include(a => a.TimeSlot)
                    .Include(a => a.Participants)
                        .ThenInclude(p => p.AppUser)
                    .Include(a => a.Team)
                    .Where(x => x.Team.Id == request.Id)
                    .ToListAsync(cancellationToken);

                var privateActivitiesToReturn = _mapper.Map<List<PrivateActivityDto>>(activities);

                return Result<List<PrivateActivityDto>>.Success(privateActivitiesToReturn);
            }
        }
    }
}