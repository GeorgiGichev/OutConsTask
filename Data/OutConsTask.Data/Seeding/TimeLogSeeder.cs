namespace OutConsTask.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OutConsTask.Data.Models;

    public class TimeLogSeeder : ISeeder
    {
        private const float MinHours = 0.25f;
        private const float MaxHours = 8f;

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.TimeLogs.Any())
            {
                return;
            }

            var logs = new HashSet<TimeLog>();
            var randomSeed = new Random();
            for (int i = 1; i <= 100; i++)
            {
                var rnd = new Random(randomSeed.Next());
                var countProjects = rnd.Next(1, 21);
                for (int j = 0; j < countProjects; j++)
                {
                    var hoursOnCurrentProject = NextFloat(MinHours, MaxHours);
                    DateTime date = GetDate(logs, i, rnd, hoursOnCurrentProject);
                    var timeLog = new TimeLog
                    {
                        UserId = i,
                        ProjectId = rnd.Next(1, 4),
                        HoursSpent = hoursOnCurrentProject,
                        Date = date,
                    };
                    logs.Add(timeLog);
                }
            }

            await dbContext.TimeLogs.AddRangeAsync(logs);
        }

        private static DateTime GetDate(HashSet<TimeLog> logs, int i, Random rnd, float hoursOnCurrentProject)
        {
            var date = RandomDate(rnd);
            var sumHours = logs
                .Where(x => x.UserId == i && x.Date.Date == date.Date)
                .Select(x => x.HoursSpent)
                .Sum();

            if (sumHours + hoursOnCurrentProject > 8)
            {
                date = GetDate(logs, i, rnd, hoursOnCurrentProject);
            }

            return date;
        }

        private static float NextFloat(float min, float max)
        {
            var random = new Random();
            double val = (random.NextDouble() * (max - min)) + min;
            var num = (float)Math.Round(val, 2);
            return num;
        }

        private static DateTime RandomDate(Random rnd)
        {
            DateTime start = new(2021, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));
        }
    }
}
