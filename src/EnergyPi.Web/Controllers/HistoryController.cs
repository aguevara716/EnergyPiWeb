using System;
using EnergyPi.Web.Builders;
using EnergyPi.Web.DataServices;
using EnergyPi.Web.Extensions;
using EnergyPi.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnergyPi.Web.Controllers
{
    public class HistoryController : Controller
    {
        // Dependencies
        private readonly IEnergyLogsDataService _energyLogsDataService;
        private readonly IHistoryViewModelBuilder _historyViewModelBuilder;

        // Constructors
        public HistoryController(IEnergyLogsDataService energyLogsDataService,
                                 IHistoryViewModelBuilder historyViewModelBuilder)
        {
            _energyLogsDataService = energyLogsDataService;
            _historyViewModelBuilder = historyViewModelBuilder;
        }

        // Private Methods
        private HistoryViewModel GetHistoryViewModelInternal(DateTime date)
        {
            var firstDayOfMonth = DateTimeExtensions.GetFirstDayOfMonth(date);
            var firstDayOfNextMonth = DateTimeExtensions.GetFirstDayOfNextMonth(date);

            var energyLogs = _energyLogsDataService.GetRecordsForDateRange(firstDayOfMonth, firstDayOfNextMonth);
            var historyViewModel = _historyViewModelBuilder.BuildViewModel(date, energyLogs);
            return historyViewModel;
        }

        // Web Methods
        public IActionResult Index(DateTime date)
        {
            var historyViewModel = GetHistoryViewModelInternal(date);
            return View(historyViewModel);
        }

        // API Methods
        [HttpGet]
        public JsonResult GetHistoryViewModel(DateTime date)
        {
            var historyViewModel = GetHistoryViewModelInternal(date);
            return Json(historyViewModel);
        }

    }
}
