using System;
using EnergyPi.Web.DataServices;
using EnergyPi.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyPi.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // Dependencies
        private readonly IEnergyLogsDataService _energyLogsDataService;

        // Constructors
        public DashboardController(IEnergyLogsDataService energyLogsDataService)
        {
            _energyLogsDataService = energyLogsDataService;
        }

        // View Methods
        public IActionResult Index()
        {
            return View();
        }

        // API Methods
        [HttpGet]
        public JsonResult GetTodaysConsumption()
        {
            var todaysConsumption = _energyLogsDataService.GetTotalConsumptionForDate(DateTime.Now);
            return Json(todaysConsumption);
        }

        [HttpGet]
        public JsonResult GetThisMonthsConsumption()
        {
            var startDate = DateTimeExtensions.GetFirstDayOfMonth(DateTime.Now);
            var endDate = DateTimeExtensions.GetFirstDayOfNextMonth(DateTime.Now);

            var thisMonthsConsumption = _energyLogsDataService.GetTotalConsumptionForDateRange(startDate, endDate);
            return Json(thisMonthsConsumption);
        }

    }
}
