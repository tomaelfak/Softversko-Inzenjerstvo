using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.TimeSlot;

namespace Application.PrivateActivities
{
    public class PrivateActivityDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Sport { get; set; }

        public string HostUsername { get; set; }

        public bool IsCancelled { get; set; }

        public int NumOfParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public int Price { get; set; }

        public TimeSlotDto TimeSlot { get; set; }

        public ICollection<ParticipantDto> Participants { get; set; }

        public string TeamName { get; set; }
    }
}