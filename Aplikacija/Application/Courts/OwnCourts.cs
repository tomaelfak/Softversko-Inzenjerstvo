using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courts
{
    public class OwnCourts
    {
        public class Query : IRequest<Result<List<CourtDto>>>
        {
            public class Handler : IRequestHandler<Query, Result<List<CourtDto>>>
            {

                private readonly DataContext _context;
                private readonly IMapper _mapper;
                private readonly IUserAccesor _userAccessor;

                public Handler(DataContext context, IMapper mapper, IUserAccesor userAccessor)
                {
                    _userAccessor = userAccessor;
                    _mapper = mapper;
                    _context = context;


                }
                public async Task<Result<List<CourtDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    var user = _userAccessor.GetUsername();




                    List<CourtDto> courts = await _context.Courts
                        .Where(x => x.Manager.UserName == user)
                        .ProjectTo<CourtDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);

                    return Result<List<CourtDto>>.Success(courts);
                }
            }
        }
    }
}