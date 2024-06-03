using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Teams;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TeamController : BaseApiController
    {

        [HttpGet("get-teams")]
        [AllowAnonymous]

        public async Task<ActionResult> GetTeams()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }


        [HttpGet("{id}/get-team")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            var result = await Mediator.Send(new Details.Query
            {
                Id = id
            });

            return HandleResult(result);
        }


        [Authorize(Roles = "Player")]
        [HttpPost("create-team")]
        public async Task<IActionResult> CreateTeam(Team team)
        {
            var result = await Mediator.Send(new Create.Command { Team = team });

            return HandleResult(result);
        }






        //[Authorize(Policy = "IsTeamOwner")]

        [AllowAnonymous]
        [HttpPut("{id}/edit-team")]
        public async Task<IActionResult> EditTeam(Guid id, Team team)
        {
            team.Id = id;
            var result = await Mediator.Send(new Edit.Command { Team = team });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}/delete-team")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command { Id = id });

            return HandleResult(result);
        }

        //[Authorize(Policy = "IsTeamOwner")]
        [AllowAnonymous]
        [HttpPost("{teamid}/{username}/add-team-member")]
        public async Task<IActionResult> AddTeamMember(Guid teamid, string username)
        {
            var result = await Mediator.Send(new AddTeamMember.Command { UserName = username, TeamId = teamid });

            return HandleResult(result);
        }

        //[Authorize(Policy = "IsTeamOwner")]
        [AllowAnonymous]
        [HttpDelete("{teamid}/{username}/remove-team-member")]
        public async Task<IActionResult> RemoveTeamMember(Guid teamid, string username)
        {
            var result = await Mediator.Send(new RemoveTeamMember.Command { TeamId = teamid, Username = username });

            return HandleResult(result);

        }
    }
}