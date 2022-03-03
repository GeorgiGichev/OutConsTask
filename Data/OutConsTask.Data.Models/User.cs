namespace OutConsTask.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OutConsTask.Data.Common.Models;

    public class User : BaseDeletableModel<int>
    {
        public User()
        {
            this.TimeLog = new HashSet<TimeLog>();
        }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Surname { get; set; }

        [Required]
        public virtual string Email { get; set; }

        public virtual ICollection<TimeLog> TimeLog { get; set; }
    }
}
