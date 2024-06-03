using Microsoft.AspNetCore.Identity;

namespace Domain
{
    //join table
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}