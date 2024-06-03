using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Teams
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Team Team { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Team).SetValidator(new TeamValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var team = await _dataContext.Teams.FindAsync(request.Team.Id);

                if (team == null) return Result<Unit>.Failure("Failed to find team");

                _mapper.Map(request.Team, team);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update team");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}