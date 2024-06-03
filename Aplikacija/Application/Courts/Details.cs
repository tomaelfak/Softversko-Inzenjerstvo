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

namespace Application.Courts
{
    public class Details
    {
        public class Query : IRequest<Result<CourtDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CourtDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;

            }
            public async Task<Result<CourtDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var court = await _context.Courts.ProjectTo<CourtDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<CourtDto>.Success(court);
            }
        }
    }
}