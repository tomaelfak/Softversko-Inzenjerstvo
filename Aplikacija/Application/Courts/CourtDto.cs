using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Profiles;

namespace Application.Courts
{
    public class CourtDto
    {
        public Guid Id { get; set; }

        public string Sport { get; set; }

        public string Name { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public string ManagerUsername { get; set; }

        public bool IsHall { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public int PricePerHour { get; set; }

        public ICollection<Event> Activities { get; set; }
    }
}