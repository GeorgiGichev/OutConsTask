namespace OutConsTask.Web.ViewModels.User
{
    using OutConsTask.Services.Data.Models;
    using OutConsTask.Services.Mapping;

    public class TopUsersViewModel : IMapFrom<TopUsersDto>
    {
        public string FullName { get; set; }

        public double TotalHours { get; set; }
    }
}
