using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TimeSlot
{
    public class TimeSlotDto
    {
        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public DateTime Day { get; set; }
    }
}