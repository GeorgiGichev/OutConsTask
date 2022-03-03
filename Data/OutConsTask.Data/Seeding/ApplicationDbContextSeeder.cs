namespace OutConsTask.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            await TruncateAllTables(dbContext);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new UserSeeder(),
                              new ProjectSeeder(),
                              new TimeLogSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }

        private static async Task TruncateAllTables(ApplicationDbContext dbContext)
        {
            var rawQuery = new StringBuilder();
            rawQuery
                .AppendLine("BEGIN TRANSACTION")
                .AppendLine("TRUNCATE TABLE Timelogs")
                .AppendLine("ALTER TABLE TimeLogs DROP CONSTRAINT FK_TimeLogs_Users_UserId")
                .AppendLine("TRUNCATE TABLE Users")
                .AppendLine("ALTER TABLE TimeLogs ADD CONSTRAINT FK_TimeLogs_Users_UserId FOREIGN KEY(UserId) REFERENCES Users (ID)")
                .AppendLine("ALTER TABLE TimeLogs DROP CONSTRAINT FK_TimeLogs_Projects_ProjectId")
                .AppendLine("TRUNCATE TABLE Projects")
                .AppendLine("ALTER TABLE TimeLogs ADD CONSTRAINT FK_TimeLogs_Projects_ProjectId FOREIGN KEY(ProjectId) REFERENCES Projects (ID)")
                .AppendLine("COMMIT TRANSACTION");

            await dbContext
                 .Database
                 .ExecuteSqlRawAsync(rawQuery.ToString());
        }
    }
}
