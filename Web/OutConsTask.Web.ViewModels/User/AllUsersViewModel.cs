namespace OutConsTask.Web.ViewModels.User
{
    using System;
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public int ItemsPerPage { get; set; }

        public int PageNumber { get; set; }

        public int CountUsers { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.UsersForListing.Count < 10 ? false : this.PageNumber < this.PageCount;

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public int PageCount => (int)Math.Ceiling((double)this.CountUsers / this.ItemsPerPage);

        public ICollection<ListedUserViewModel> UsersForListing { get; set; }
    }
}
