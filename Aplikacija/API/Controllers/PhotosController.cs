using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPost("addMain")]
        public async Task<IActionResult> AddMain([FromForm] AddMain.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
        
        [HttpDelete("deleteMain")]
        public async Task<IActionResult> DeleteMain()
        {
            return HandleResult(await Mediator.Send(new DeleteMain.Command()));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command { Id = id }));
        }
    }
}