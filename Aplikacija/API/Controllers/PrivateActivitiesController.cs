using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.PrivateActivities;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PrivateActivitiesController : BaseApiController
    {
        [HttpGet("{id}/privateactivities")]
        [AllowAnonymous]

        public async Task<ActionResult> GetPrivateActivities(Guid Id)
        {
            return HandleResult(await Mediator.Send(new List.Query
            {
                Id = Id
            }));
        }

        [AllowAnonymous]
        [HttpGet("{activityid}/privateactivity")]
        public async Task<IActionResult> GetPrivateActivity(Guid activityid)
        {
            var result = await Mediator.Send(new Details.Query
            {
                Id = activityid
            });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpPost("{TeamId}/{CourtId}/create-private-activity")]
        public async Task<IActionResult> CreatePrivateActivity(Guid TeamId, Guid CourtId, PrivateActivity privateActivity, int StartTime, int EndTime)
        {
            var result = await Mediator.Send(new Create.Command { PrivateActivity = privateActivity, TeamId = TeamId, CourtId = CourtId, StartTime = StartTime, EndTime = EndTime });

            return HandleResult(result);
        }


        [AllowAnonymous]
        [HttpPut("{id}/edit-private-activity")]
        public async Task<IActionResult> EditPrivateActivity(Guid id, PrivateActivity privateActivity, int StartTime, int EndTime)
        {
            privateActivity.Id = id;
            var result = await Mediator.Send(new Edit.Command { PrivateActivity = privateActivity, StartTime = StartTime, EndTime = EndTime });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}/delete-private-activity")]
        public async Task<IActionResult> DeletePrivateActivity(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command { Id = id });

            return HandleResult(result);
        }


        [AllowAnonymous]
        [HttpPost("{activityid}/join")]
        public async Task<IActionResult> Attend(Guid activityid)
        {
            var result = await Mediator.Send(new UpdateParticipant.Command { Id = activityid });

            return HandleResult(result);
        }
    }
}