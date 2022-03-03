namespace OutConsTask.Web.ViewModels.User
{
    using OutConsTask.Data.Models;
    using OutConsTask.Services.Mapping;

    public class ListedUserViewModel : IMapFrom<User>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
