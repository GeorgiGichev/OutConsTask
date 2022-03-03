namespace OutConsTask.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using OutConsTask.Services.Data;
    using OutConsTask.Web.ViewModels;
    using OutConsTask.Web.ViewModels.User;

    public class HomeController : BaseController
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index(int id = 1)
        {
            var users = this.userService.GetAll<ListedUserViewModel>(id, DateTime.MinValue, DateTime.UtcNow);
            var viewModel = new AllUsersViewModel
            {
                PageNumber = id,
                ItemsPerPage = 10,
                CountUsers = this.userService.GetCount(),
                UsersForListing = users as ICollection<ListedUserViewModel>,
            };
            return this.View(viewModel);
        }

        [HttpGet]
        public JsonResult AllUsers(int id, DateTime? startDate, DateTime? lastDate)
        {
            if (startDate is null)
            {
                startDate = DateTime.MinValue;
            }

            if (lastDate is null)
            {
                lastDate = DateTime.UtcNow;
            }

            const int ItemsPerPage = 10;
            var viewModel = new AllUsersViewModel
            {
                PageNumber = id,
                ItemsPerPage = ItemsPerPage,
                CountUsers = this.userService.GetCount(),
                UsersForListing = this.userService
                .GetAll<ListedUserViewModel>(id, startDate, lastDate) as ICollection<ListedUserViewModel>,
            };

            return this.Json(viewModel);
        }

        [HttpGet]
        public JsonResult GetTop10Users(DateTime? startDate, DateTime? lastDate)
        {
            if (startDate is null)
            {
                startDate = DateTime.MinValue;
            }

            if (lastDate is null)
            {
                lastDate = DateTime.UtcNow;
            }

            var result = this.userService.GetTop10ByDates<TopUsersViewModel>(startDate, lastDate);
            return this.Json(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
