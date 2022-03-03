namespace OutConsTask.Web.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using OutConsTask.Services.Data;
    using OutConsTask.Web.ViewModels.Projects;
    using OutConsTask.Web.ViewModels.User;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public JsonResult HoursByUser(int userId)
        {
            var result = this.projectService.GetAllByUserId<ProjectsHoursViewModel>(userId);
            return this.Json(result);
        }

        [HttpGet]
        public JsonResult GetTop10Projects(DateTime? startDate, DateTime? lastDate)
        {
            if (startDate is null)
            {
                startDate = DateTime.MinValue;
            }

            if (lastDate is null)
            {
                lastDate = DateTime.UtcNow;
            }

            var result = this.projectService.GetTop10ProjetsByDate<TopProjectsViewModel>(startDate, lastDate);
            return this.Json(result);
        }
    }
}
