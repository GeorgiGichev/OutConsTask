namespace OutConsTask.Data.Models
{
    using System;

    using OutConsTask.Data.Common.Models;

    public class TimeLog : BaseDeletableModel<int>
    {
        public virtual int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual float HoursSpent { get; set; }
    }
}
