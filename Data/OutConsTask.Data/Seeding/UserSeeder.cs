namespace OutConsTask.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using OutConsTask.Data.Models;

    internal class UserSeeder : ISeeder
    {
        private readonly IList<string> names = new string[]
        {
            "John",
            "Gringo",
            "Mark", "Lisa",
            "Maria",
            "Sonya",
            "Philip",
            "Jose",
            "Lorenzo",
            "George",
            "Justin",
        };

        private readonly IList<string> surnames = new string[]
        {
            "Johnson",
            "Lamas",
            "Jackson",
            "Brown",
            "Mason",
            "Rodriguez",
            "Roberts",
            "Thomas",
            "Rose",
            "McDonalds",
        };

        private readonly IList<string> domains = new string[]
        {
            "hotmail.com",
            "gmail.com",
            "live.com",
        };

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var randomSeed = new Random();
            var users = new HashSet<User>();

            for (int i = 0; i < 100; i++)
            {
                var rnd = new Random(randomSeed.Next());
                var name = this.names[rnd.Next(this.names.Count - 1)];
                var surname = this.surnames[rnd.Next(this.surnames.Count - 1)];
                var email = $"{name}.{surname}@{this.domains[rnd.Next(this.domains.Count - 1)]}";
                var user = new User
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                };

                users.Add(user);
            }

            await dbContext.Users.AddRangeAsync(users);
        }
    }
}
