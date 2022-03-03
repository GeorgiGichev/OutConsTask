namespace OutConsTask.Web.ViewModels.User
{
    using OutConsTask.Services.Data.Models;
    using OutConsTask.Services.Mapping;

    public class ProjectsHoursViewModel : IMapFrom<AllProjectsByUserDto>
    {
        public string UserNames { get; set; }

        public string ProjectName { get; set; }

        public string Hours { get; set; }
    }
}
