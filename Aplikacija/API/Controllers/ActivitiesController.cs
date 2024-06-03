using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {


        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Player")]
        public async Task<ActionResult> GetActivites()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var result = await Mediator.Send(new Details.Query
            {
                Id = id
            });

            return HandleResult(result);
        }

        //[AllowAnonymous]
        [Authorize(Roles = "Player")]
        [HttpPost("{CourtId}")]
        public async Task<IActionResult> CreateActivity(Activity activity, Guid CourtId, int StartTime, int EndTime) // moze [FromBody] ali je dovoljno pametan sam da shvati
        {
            var result = await Mediator.Send(new Create.Command { Activity = activity, CourtId = CourtId, StartTime = StartTime, EndTime = EndTime });

            return HandleResult(result);
        }

        //[Authorize(Policy = "IsActivityHost")]
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity, int StartTime, int EndTime)
        {
            activity.Id = id;
            var result = await Mediator.Send(new Edit.Command { Activity = activity, StartTime = StartTime, EndTime = EndTime });

            return HandleResult(result);
        }
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command { id = id });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpPost("{id}/participate")]
        public async Task<IActionResult> Attend(Guid id)
        {
            var result = await Mediator.Send(new UpdateParticipants.Command { Id = id });

            return HandleResult(result);
        }

    }
}