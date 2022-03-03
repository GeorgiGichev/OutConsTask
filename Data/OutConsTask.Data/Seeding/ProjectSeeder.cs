namespace OutConsTask.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OutConsTask.Data.Models;

    internal class ProjectSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Projects.Any())
            {
                return;
            }

            var projects = new HashSet<Project>()
            {
                new Project
                {
                    Name = "My own",
                },
                new Project
                {
                    Name = "Outcons",
                },
                new Project
                {
                    Name = "Free Time",
                },
            };
            await dbContext.Projects.AddRangeAsync(projects);
        }
    }
}
