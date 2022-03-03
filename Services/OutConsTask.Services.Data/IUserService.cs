namespace OutConsTask.Services.Data
{
    using System;
    using System.Collections.Generic;

    public interface IUserService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>(int page, DateTime? startDate, DateTime? lastDate, int itemsPerPage = 10);

        IEnumerable<T> GetTop10ByDates<T>(DateTime? startDate, DateTime? lastdate);
    }
}
