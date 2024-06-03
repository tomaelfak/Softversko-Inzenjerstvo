using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Score  { get; set; }

        public string Comment { get; set; }

        public Guid RatedByUserId { get; set; }
        public AppUser RatedByUser { get; set; }

        public Guid RatedUserId { get; set; }

        public AppUser RatedUser { get; set; }
    }
}