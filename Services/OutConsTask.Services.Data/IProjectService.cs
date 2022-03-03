namespace OutConsTask.Services.Data
{
    using System;
    using System.Collections.Generic;

    public interface IProjectService
    {
        IEnumerable<T> GetAllByUserId<T>(int userId);

        IEnumerable<T> GetTop10ProjetsByDate<T>(DateTime? startDate, DateTime? lastDate);
    }
}
