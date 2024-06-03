using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Profiles
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Sport { get; set; }

        public string Description { get; set; }

        public AppUser AppUser { get; set; }

        public string HostName { get; set; }

        public int NumOfParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public Domain.TimeSlot TimeSlot { get; set; }

        public ICollection<Profile> Participants { get; set; }
    }
}