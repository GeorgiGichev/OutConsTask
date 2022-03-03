namespace OutConsTask.Services.Data.Models
{
    using OutConsTask.Data.Models;
    using OutConsTask.Services.Mapping;

    public class AllProjectsByUserDto
    {
        public string UserNames { get; set; }

        public string ProjectName { get; set; }

        public double Hours { get; set; }
    }
}
