using Application.Profiles;
using Application.TimeSlot;
using Domain;

namespace Application.Activities
{
    public class ActivityDto
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
    }
}