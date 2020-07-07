using System;
using System.Linq;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Repositories;

namespace EnergyPi.Web.DataServices
{
    public interface IWeatherLogsDataService
    {
        IQueryable<WeatherLogs> GetRecordsForDate(DateTime date);
        IQueryable<WeatherLogs> GetRecordsForDateRange(DateTime startDate, DateTime endDate);
    }

    public class WeatherLogsDataService : IWeatherLogsDataService
    {
        // Dependencies
        private readonly IRepository<WeatherLogs> _weatherLogsRepository;

        // Constructors
        public WeatherLogsDataService(IRepository<WeatherLogs> weatherLogsRepository)
        {
            _weatherLogsRepository = weatherLogsRepository;
        }

        // Get Records
        public IQueryable<WeatherLogs> GetRecordsForDate(DateTime date)
        {
            var nextDay = date.Date.AddDays(1);

            var records = GetRecordsForDateRange(date, nextDay);
            return records;
        }

        public IQueryable<WeatherLogs> GetRecordsForDateRange(DateTime startDate, DateTime endDate)
        {
            var records = _weatherLogsRepository.Get(wl => wl.Timestamp.Date >= startDate.Date && wl.Timestamp.Date < endDate.Date);
            return records;
        }

    }
}
