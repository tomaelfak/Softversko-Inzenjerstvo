using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messages;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        public async Task SendMessage(Create.Command command)
        {
            var result = await _mediator.Send(command);
            await Clients.Group(command.TeamId.ToString()).SendAsync("ReceiveMessage", result.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var teamId = httpContext.Request.Query["teamId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);
            var result = await _mediator.Send(new List.Query{TeamId = Guid.Parse(teamId)});
            await Clients.Caller.SendAsync("LoadMessages", result.Value);
        }
    }
}