using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Court
    {
        public Guid Id { get; set; }

        public string Sport { get; set; }

        public string Name { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public AppUser Manager { get; set; }

        public Guid ManagerId { get; set; }

        public bool IsHall { get; set; } = false;

        public int PricePerHour { get; set; } = 0;

        public int StartTime { get; set; } = 0;
        public int EndTime { get; set; } = 24;

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

        


    }
}