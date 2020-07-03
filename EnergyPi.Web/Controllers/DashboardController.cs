using System;
using System.Linq;
using EnergyPi.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyPi.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DataDbContext _dataDbContext;

        public DashboardController(DataDbContext dataDbContext)
        {
            _dataDbContext = dataDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTodaysConsumption()
        {
            var todaysConsumption = _dataDbContext.EnergyLogs.Where(el => el.Timestamp.Date == DateTime.Now.Date).Sum(el => el.Delta);
            return Json(todaysConsumption);
        }

    }
}
