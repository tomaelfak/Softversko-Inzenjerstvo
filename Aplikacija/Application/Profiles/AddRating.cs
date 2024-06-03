using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class AddRating
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Username { get; set; }

            public int Score { get; set; }

            public string Comment   { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccesor _userAccesor;
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext, IUserAccesor userAccesor)
            {
                _dataContext = dataContext;
                _userAccesor = userAccesor;

            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var usergiving = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                var userreceiving = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);

                if (userreceiving == null)
                {
                    return Result<Unit>.Failure("User not found!");
                }

                if (usergiving == userreceiving)
                {
                    return Result<Unit>.Failure("You can't rate yourself!");
                }

                var rating = await _dataContext.Ratings.FirstOrDefaultAsync(x => x.RatedByUserId == usergiving.Id && x.RatedUserId == userreceiving.Id);

                if (rating != null)
                {
                    return Result<Unit>.Failure("You have already rated this user!");
                }

                rating = new Rating
                {
                    RatedByUser = usergiving,
                    RatedUser = userreceiving,
                    RatedByUserId = usergiving.Id,
                    RatedUserId = userreceiving.Id,
                    Score = request.Score,
                    Comment = request.Comment
                };

                _dataContext.Ratings.Add(rating);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to add rating!");
                }

                return Result<Unit>.Success(Unit.Value);

                
            }
        }
    }
}