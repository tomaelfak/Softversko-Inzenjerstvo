using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Messages
{
    public class Create
    {
        public class Command : IRequest<Result<MessageDto>>
        {
            public string Body { get; set; }

            public Guid TeamId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<MessageDto>>
        {
            private readonly DataContext _context;
            private readonly IUserAccesor _userAccessor;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IUserAccesor userAccessor, IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<Result<MessageDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var team = await _context.Teams.FindAsync(request.TeamId);

                if (team == null) return null;

                var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var message = new Message
                {
                    Author = user,
                    Team = team,
                    Body = request.Body
                };

                team.Messages.Add(message);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<MessageDto>.Success(_mapper.Map<MessageDto>(message));

                return Result<MessageDto>.Failure("Failed to add message");
            }
        }
    }
}