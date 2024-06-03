using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Challenge
    {
        public Guid Id { get; set; }

        public Guid ChallengerTeamId { get; set; }

        public Guid ChallengedTeamId { get; set; }

        public ChallengeStatus Status { get; set; }

        public Guid CourtId { get; set; }

        public TimeSlot TimeSlot { get; set; }
    }

    public enum ChallengeStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}