namespace OutConsTask.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OutConsTask.Data.Common.Repositories;
    using OutConsTask.Data.Models;
    using OutConsTask.Services.Data.Models;
    using OutConsTask.Services.Mapping;

    public class PojectService : IProjectService
    {
        private readonly IDeletableEntityRepository<TimeLog> timeLogRepository;

        public PojectService(IDeletableEntityRepository<TimeLog> timeLogRepository)
        {
            this.timeLogRepository = timeLogRepository;
        }

        public IEnumerable<T> GetAllByUserId<T>(int userId)
        {
            var data = this.timeLogRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new AllProjectsByUserDto
                {
                    ProjectName = x.Project.Name,
                    Hours = x.HoursSpent,
                    UserNames = x.User.Name + " " + x.User.Surname,
                })
                .AsEnumerable()
                .GroupBy(x => x.ProjectName);

            var result = new List<AllProjectsByUserDto>();
            foreach (var projects in data)
            {
                var project = new AllProjectsByUserDto
                {
                    ProjectName = projects.Key,
                    Hours = projects.Select(x => x.Hours).Sum(),
                    UserNames = projects.FirstOrDefault().UserNames,
                };
                result.Add(project);
            }

            return result.AsQueryable().To<T>().ToList();
        }

        public IEnumerable<T> GetTop10ProjetsByDate<T>(DateTime? startDate, DateTime? lastDate)
        {
            var data = this.timeLogRepository
                .AllAsNoTracking()
                .Where(x => x.Date >= startDate.Value && x.Date <= lastDate.Value)
                .Select(x => new TopProjectsDto
                {
                    UserId = x.UserId,
                    ProjectName = x.Project.Name,
                    Hours = x.HoursSpent,
                    UserName = x.User.Name + " " + x.User.Surname,
                })
                .AsEnumerable()
                .GroupBy(x => new { x.UserId, x.ProjectName })
                .OrderByDescending(x => x.Sum(y => y.Hours))
                .Take(10)
                .ToList();

            var result = new List<TopProjectsDto>();
            foreach (var projects in data)
            {
                var project = new TopProjectsDto
                {
                    ProjectName = projects.Key.ProjectName,
                    Hours = projects.Select(x => x.Hours).Sum(),
                    UserName = projects.FirstOrDefault().UserName,
                };
                result.Add(project);
            }

            return result.AsQueryable().To<T>().ToList();
        }
    }
}
