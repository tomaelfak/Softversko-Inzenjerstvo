using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Profiles
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Image { get; set; }

        public string Bio { get; set; }

        public string Address { get; set; }

        public List<RatingDto> ReceivedRatings { get; set; }
    }
}