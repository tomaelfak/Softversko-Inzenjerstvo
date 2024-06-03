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
    public class List
    {
        public class Query : IRequest<Result<List<CourtDto>>>
        {
            public class Handler : IRequestHandler<Query, Result<List<CourtDto>>>
            {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

                public Handler(DataContext context, IMapper mapper)
                {
                    _mapper = mapper;
                    _context = context;
                    
                }

                public async Task<Result<List<CourtDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    var courts = await _context.Courts.ProjectTo<CourtDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

                    return Result<List<CourtDto>>.Success(courts);
                }
            }
        }

    }
}