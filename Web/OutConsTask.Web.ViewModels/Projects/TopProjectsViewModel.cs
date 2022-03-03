namespace OutConsTask.Web.ViewModels.Projects
{
    using AutoMapper;

    using OutConsTask.Services.Data.Models;
    using OutConsTask.Services.Mapping;

    public class TopProjectsViewModel : IMapFrom<TopProjectsDto>, IHaveCustomMappings
    {
        public string FullName { get; set; }

        public double TotalHours { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<TopProjectsDto, TopProjectsViewModel>()
                .ForMember(x => x.TotalHours, opt =>
                opt.MapFrom(x => (double)x.Hours))
                .ForMember(x => x.FullName, opt =>
                opt.MapFrom(x => x.ProjectName + " - " + x.UserName));
        }
    }
}
