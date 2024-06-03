using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace Domain


{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public string Address { get; set; }

        public Photo Image { get; set; } 

        public ICollection<AppUserRole> UserRoles { get; set; }

        public ICollection<ActivityParticipant> Activities { get; set; }


        public ICollection<Photo> Photos { get; set; }

        public ICollection<Rating> GivenRatings { get; set; }

        public ICollection<Rating> ReceivedRatings { get; set; }

        


        public ICollection<TeamParticipant> Teams { get; set; }



    }
}