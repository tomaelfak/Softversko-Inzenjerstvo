using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Profiles;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{username}/profile")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }


        [Authorize(Roles = "Player")]
        [HttpPost("{username}/rate")]
        public async Task<IActionResult> AddRating(string username, [FromBody] RatingDto rating)
        {
            var result = await Mediator.Send(new AddRating.Command { Username = username, Score = rating.Score, Comment = rating.Comment });
            return HandleResult(result);
        }


        [Authorize(Roles = "Player")]
        [HttpGet("{userId}/rating")]
        public async Task<IActionResult> GetRating(Guid userId)
        {
            return HandleResult(await Mediator.Send(new ShowRating.Query { UserId = userId }));
        }

        [Authorize(Roles = "Player")]
        [HttpGet("/profiles")]
        public async Task<IActionResult> GetProfiles()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        //[Authorize(Roles = "Player")]
        [AllowAnonymous]
        [HttpGet("/search")]
        public async Task<IActionResult> SearchProfiles(string username)
        {
            return HandleResult(await Mediator.Send(new Search.Query { Username = username }));
        }

        [AllowAnonymous]
        [HttpPut("/edit")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest request)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Bio = request.Bio }));
        }
    }

    public class EditProfileRequest
    {
        public string Bio { get; set; }
    }
}