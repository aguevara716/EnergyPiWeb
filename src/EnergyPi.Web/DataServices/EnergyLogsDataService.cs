using System;
using System.Linq;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Repositories;

namespace EnergyPi.Web.DataServices
{
    public interface IEnergyLogsDataService
    {
        // Get Records
        IQueryable<EnergyLogs> GetRecordsForDate(DateTime date);
        IQueryable<EnergyLogs> GetRecordsForDateRange(DateTime startDate, DateTime endDate);
    }

    public class EnergyLogsDataService : IEnergyLogsDataService
    {
        // Dependencies
        private readonly IRepository<EnergyLogs> _energyLogsRepository;

        // Constructors
        public EnergyLogsDataService(IRepository<EnergyLogs> energyLogsRepository)
        {
            _energyLogsRepository = energyLogsRepository;
        }

        // Get Records
        public IQueryable<EnergyLogs> GetRecordsForDate(DateTime date)
        {
            var nextDay = date.Date.AddDays(1);

            var records = GetRecordsForDateRange(date, nextDay);
            return records;
        }

        public IQueryable<EnergyLogs> GetRecordsForDateRange(DateTime startDate, DateTime endDate)
        {
            var records = _energyLogsRepository.Get(el => el.Timestamp.Date >= startDate.Date && el.Timestamp.Date < endDate.Date);
            return records;
        }

    }
}
