using System;
using EnergyPi.Web.Builders;
using EnergyPi.Web.DataServices;
using EnergyPi.Web.Extensions;
using EnergyPi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyPi.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // Dependencies
        private readonly IDashboardViewModelBuilder _dashboardViewModelBuilder;
        private readonly IEnergyLogsDataService _energyLogsDataService;

        // Constructors
        public DashboardController(IDashboardViewModelBuilder dashboardViewModelBuilder,
                                   IEnergyLogsDataService energyLogsDataService)
        {
            _dashboardViewModelBuilder = dashboardViewModelBuilder;
            _energyLogsDataService = energyLogsDataService;
        }

        // Private Methods
        private DashboardViewModel GetDashboardViewModelInternal()
        {
            var startDate = DateTimeExtensions.GetFirstDayOfMonth(DateTime.Now);
            var endDate = DateTimeExtensions.GetFirstDayOfNextMonth(DateTime.Now);

            var currentMonthsEnergyLogs = _energyLogsDataService.GetRecordsForDateRange(startDate, endDate);

            var dashboardViewModel = _dashboardViewModelBuilder.BuildDashboardViewModel(currentMonthsEnergyLogs);
            return dashboardViewModel;
        }

        // View Methods
        public IActionResult Index()
        {
            var dashboardViewModel = GetDashboardViewModelInternal();
            return View(dashboardViewModel);
        }

        // API Methods
        [HttpGet]
        public JsonResult GetDashboardViewModel()
        {
            var dashboardViewModel = GetDashboardViewModelInternal();
            return Json(dashboardViewModel);
        }

    }
}
