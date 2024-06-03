using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class TimeSlot
    {
        public Guid Id { get; set; }

        public DateTime Day { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public Guid CourtId { get; set; }

        

        
    }
}