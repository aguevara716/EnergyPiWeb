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
    public class TodayController : Controller
    {
        // Dependencies
        private readonly ITodayViewModelBuilder _todayViewModelBuilder;
        private readonly IEnergyLogsDataService _energyLogsDataService;
        private readonly IWeatherLogsDataService _weatherLogsDataService;

        // Constructors
        public TodayController(ITodayViewModelBuilder todayViewModelBuilder,
                               IEnergyLogsDataService energyLogsDataService,
                               IWeatherLogsDataService weatherLogsDataService)
        {
            _todayViewModelBuilder = todayViewModelBuilder;
            _energyLogsDataService = energyLogsDataService;
            _weatherLogsDataService = weatherLogsDataService;
        }

        // Private Methods
        private TodayViewModel GetTodayViewModelInternal()
        {
            var startDate = DateTimeExtensions.GetFirstDayOfMonth(DateTime.Now);
            var endDate = DateTimeExtensions.GetFirstDayOfNextMonth(DateTime.Now);

            var currentMonthsEnergyLogs = _energyLogsDataService.GetRecordsForDateRange(startDate, endDate);
            var currentMonthsWeatherLogs = _weatherLogsDataService.GetRecordsForDateRange(startDate, endDate);

            var todayViewModel = _todayViewModelBuilder.BuildTodayViewModel(currentMonthsEnergyLogs, currentMonthsWeatherLogs);
            return todayViewModel;
        }

        // View Methods
        public IActionResult Index()
        {
            var todayViewModel = GetTodayViewModelInternal();
            return View(todayViewModel);
        }

        // API Methods
        [HttpGet]
        public JsonResult GetTodayViewModel()
        {
            var todayViewModel = GetTodayViewModelInternal();
            return Json(todayViewModel);
        }

    }
}
