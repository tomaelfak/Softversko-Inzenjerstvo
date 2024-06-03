using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Team
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Sport  { get; set; }

        public int Wins { get; set; } = 0;

        public int Losses { get; set; } = 0;

        public int NumberOfTeammates { get; set; } = 1;

        public ICollection<TeamParticipant> Participants { get; set; } = new List<TeamParticipant>();

        public ICollection<PrivateActivity> PrivateActivities { get; set; } = new List<PrivateActivity>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();


    }
}