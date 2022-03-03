namespace OutConsTask.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OutConsTask.Data.Common.Repositories;
    using OutConsTask.Data.Models;
    using OutConsTask.Services.Data.Models;
    using OutConsTask.Services.Mapping;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<User> userRepo;

        public UserService(IDeletableEntityRepository<User> userRepo)
        {
            this.userRepo = userRepo;
        }

        public int GetCount()
        {
            return this.userRepo.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>(int page, DateTime? startDate, DateTime? lastDate, int itemsPerPage = 10)
        {
            return this.userRepo.AllAsNoTracking()
                .Where(x => x.TimeLog.Any(y => y.Date.Date >= startDate.Value.Date && y.Date.Date <= lastDate.Value.Date))
                .OrderBy(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetTop10ByDates<T>(DateTime? startDate, DateTime? lastdate)
        {
            var result = this.userRepo.AllAsNoTracking()
               .Where(x => x.TimeLog.Any(y => y.Date.Date >= startDate.Value.Date
                                           && y.Date.Date <= lastdate.Value.Date))
               .OrderByDescending(x => x.TimeLog
                                        .Where(y => y.Date.Date >= startDate.Value.Date
                                                && y.Date.Date <= lastdate.Value.Date)
                                        .Sum(y => y.HoursSpent))
               .Take(10)
               .Select(x => new TopUsersDto
               {
                   FullName = x.Name + " " + x.Surname,
                   TotalHours = x.TimeLog
                                    .Where(y => y.Date.Date >= startDate.Value.Date
                                             && y.Date.Date <= lastdate.Value.Date)
                                    .Sum(y => y.HoursSpent),
               })
               .To<T>()
               .ToList();

            return result;
        }
    }
}
