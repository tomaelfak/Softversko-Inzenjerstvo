using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Message
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public AppUser Author { get; set; }

        public Team Team { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}