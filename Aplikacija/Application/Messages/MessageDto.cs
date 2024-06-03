using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class MessageDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Body { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Image { get; set; }
    }
}