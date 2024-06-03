namespace Domain
{
    public class Activity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Sport { get; set; }

        public bool IsCancelled { get; set; }

        public int NumOfParticipants { get; set; } = 1;
        public int MaxParticipants { get; set; }

        public int Price { get; set; }

        public virtual ICollection<ActivityParticipant> Participants { get; set; } = new List<ActivityParticipant>();

        public TimeSlot TimeSlot { get; set; }

        public Guid TimeSlotId { get; set; }

        public Guid CourtId { get; set; }
        public Court Court { get; set; }

        
    }
}