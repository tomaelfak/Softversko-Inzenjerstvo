using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ShowRating
    {
        public class Query : IRequest<Result<float>>
        {
            public Guid UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<float>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<float>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ratings = await _context.Ratings.Where(x => x.RatedUserId == request.UserId).ToListAsync();

                if (ratings.Count == 0) return Result<float>.Success(0);

                var sum = ratings.Sum(x => x.Score);
                float average = sum / ratings.Count;

                return Result<float>.Success(average);
            }
        }
    }
}