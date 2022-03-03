namespace OutConsTask.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OutConsTask.Data.Common.Models;

    public class Project : BaseDeletableModel<int>
    {
        public Project()
        {
            this.TimeLog = new HashSet<TimeLog>();
        }

        [Required]
        public virtual string Name { get; set; }

        public virtual ICollection<TimeLog> TimeLog { get; set; }
    }
}
