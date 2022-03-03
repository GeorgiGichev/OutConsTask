namespace OutConsTask.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using OutConsTask.Data.Models;

    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> project)
        {
            project
                .HasMany(e => e.TimeLog)
                .WithOne()
                .HasForeignKey(e => e.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
