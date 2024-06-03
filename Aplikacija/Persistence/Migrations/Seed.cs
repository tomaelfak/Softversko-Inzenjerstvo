using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {

            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{DisplayName = "Bob", UserName = "bob", Email = "bob@test.com", Address = "Beograd" },
                    new AppUser{DisplayName = "Tom", UserName = "tom", Email = "tom@test.com", Address = "Pirot" },
                    new AppUser{DisplayName = "Jane", UserName = "jane", Email = "jane@test.com" , Address = "Nis" }
                };

                var roles = new List<AppRole>
                {
                    new AppRole{Name = "Player"},
                    new AppRole{Name = "Admin"},
                    new AppRole{Name = "Manager"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");

                    await userManager.AddToRoleAsync(user, "Player");
                }

                var admin = new AppUser { DisplayName = "Steva", UserName = "steva", Email = "steva@admin.com", Address = "Pirot" };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRoleAsync(admin, "Admin");

                var manager = new AppUser { DisplayName = "Sveta", UserName = "sveta", Email = "sveta@admin.com", Address = "Pirot" };
                await userManager.CreateAsync(manager, "Pa$$w0rd");
                await userManager.AddToRoleAsync(manager, "Manager");

            }




            // if (context.Activities.Any()) return;

            // var activities = new List<Activity>
            // {
            //     new Activity
            //     {
            //         Title = "Past Activity 1",
            //         Date = DateTime.UtcNow.AddMonths(-2),
            //         Description = "Activity 2 months ago",
            //         Sport = "Football"
            //     },
            //     new Activity
            //     {
            //         Title = "Past Activity 2",
            //         Date = DateTime.UtcNow.AddMonths(-1),
            //         Description = "Activity 1 month ago",
            //         Sport = "Basketball"
            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 1",
            //         Date = DateTime.UtcNow.AddMonths(1),
            //         Description = "Activity 1 month in future",
            //         Sport = "Football",

            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 2",
            //         Date = DateTime.UtcNow.AddMonths(2),
            //         Description = "Activity 2 months in future",
            //         Sport = "Tennis",

            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 3",
            //         Date = DateTime.UtcNow.AddMonths(3),
            //         Description = "Activity 3 months in future",
            //         Sport = "Basketball"
            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 4",
            //         Date = DateTime.UtcNow.AddMonths(4),
            //         Description = "Activity 4 months in future",
            //         Sport = "Football"
            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 5",
            //         Date = DateTime.UtcNow.AddMonths(5),
            //         Description = "Activity 5 months in future",
            //         Sport = "Chess"
            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 6",
            //         Date = DateTime.UtcNow.AddMonths(6),
            //         Description = "Activity 6 months in future",
            //         Sport = "Badminton"
            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 7",
            //         Date = DateTime.UtcNow.AddMonths(7),
            //         Description = "Activity 2 months ago",
            //         Sport = "Hockey"
            //     },
            //     new Activity
            //     {
            //         Title = "Future Activity 8",
            //         Date = DateTime.UtcNow.AddMonths(8),
            //         Description = "Activity 8 months in future",
            //         Sport = "Futsal"
            //     }
            // };

            // await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
        }
    }
}