using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Courts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class CourtController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCourt(Court court)
        {
            var result = await Mediator.Send(new Create.Command { Court = court });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourt(Guid id)
        {
            var result = await Mediator.Send(new Details.Query
            {
                Id = id
            });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCourts()
        {
            var result = await Mediator.Send(new List.Query());

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCourt(Guid id, Court court)
        {
            court.Id = id;
            var result = await Mediator.Send(new Edit.Command { Court = court });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourt(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command { id = id });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}/scheduledslots")]
        public async Task<IActionResult> GetScheduledSlots(Guid id, DateTime date)
        {
            var result = await Mediator.Send(new ScheduledSlots.Query
            {
                Id = id,
                Date = date
            });

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet("OwnCourts")]
        public async Task<IActionResult> OwnCourts()
        {
            var result = await Mediator.Send(new OwnCourts.Query());

            return HandleResult(result);
        }

    }
}