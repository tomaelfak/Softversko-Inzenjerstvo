using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PrivateActivities
{
    public class Details
    {
        public class Query : IRequest<Result<PrivateActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PrivateActivityDto>>
        {
            private readonly IMapper _mapper;
            private readonly DataContext _context;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;

                _mapper = mapper;
            }
            public async Task<Result<PrivateActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.PrivateActivities
                    .ProjectTo<PrivateActivityDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<PrivateActivityDto>.Success(activity);
            }
        }
    }
}