using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Persistence;

namespace Infrastructure.Security
{
    public class IsCourtOwnerRequirement : IAuthorizationRequirement
    {

    }


    public class IsCourtOwnerRequirementHandler : AuthorizationHandler<IsCourtOwnerRequirement>
    {
        private readonly DataContext _dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsCourtOwnerRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCourtOwnerRequirement requirement)
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