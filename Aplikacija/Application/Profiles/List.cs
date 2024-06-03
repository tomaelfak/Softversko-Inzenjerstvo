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

namespace Application.Profiles
{
    public class List
    {
        public class Query : IRequest<Result<List<ProfileDto>>>
        {
            public class Handler : IRequestHandler<Query, Result<List<ProfileDto>>>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public Handler(DataContext context, IMapper mapper)
                {
                    _mapper = mapper;
                    _context = context;
                }

                public async Task<Result<List<ProfileDto>>> Handle(Query request, CancellationToken cancellationToken)
                {


                    var profiles = await _context.Users
                        .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Player"))
                        .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);

                    return Result<List<ProfileDto>>.Success(profiles);
                }
            }
        }
    }
}