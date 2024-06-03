namespace Domain
{
    public class TeamParticipant
    {
        public Guid AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public Guid TeamId { get; set; }

        public Team Team { get; set; }

        public bool IsCaptain { get; set; }

        
    }
}