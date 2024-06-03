using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsTeamOwnerRequirement : IAuthorizationRequirement
    {
        
    }

    public class IsTeamOwnerRequirementHandler: AuthorizationHandler<IsTeamOwnerRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsTeamOwnerRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsTeamOwnerRequirement requirement)
        {
            var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == Guid.Empty)
                return Task.CompletedTask;

            var TeamId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString());

            var participant = _dbContext.TeamParticipants
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUserId == userId && x.TeamId == TeamId).Result;

            if (participant == null)
                return Task.CompletedTask;

            if (participant.IsCaptain)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}