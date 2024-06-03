using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;

namespace Application.Teams
{
    public class TeamDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Sport { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int NumberOfTeammates { get; set; } = 1;

        public string CaptainUsername { get; set; }

        public ICollection<ParticipantDto> Participants { get; set; }
    }
}