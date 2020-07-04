using System;
using System.Collections.Generic;
using System.Linq;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Repositories;

namespace EnergyPi.Web.DataServices
{
    public interface IEnergyLogsDataService
    {
        // Get Total Consumption
        decimal? GetTotalConsumptionForDate(DateTime date);
        decimal? GetTotalConsumptionForDateRange(DateTime startDate, DateTime endDate);

        // Get Each Days' Consumption
        Dictionary<DateTime, Decimal?> GetEachDaysConsumptionForDateRange(DateTime startDate, DateTime endDate);

        // Get Each Days' Records
        Dictionary<DateTime, IEnumerable<EnergyLogs>> GetEachDaysRecordsForDateRange(DateTime startDate, DateTime endDate);

        // Get Records
        IQueryable<EnergyLogs> GetRecordsForDate(DateTime date);
        IQueryable<EnergyLogs> GetRecordsForDateRange(DateTime startDate, DateTime endDate);

        // Get last broadcast
        EnergyLogs GetLastBroadcastFromDate(DateTime dateTime);
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

        // Get Total Consumption
        public decimal? GetTotalConsumptionForDate(DateTime date)
        {
            var consumption = GetRecordsForDate(date).Sum(el => el.Delta);
            return consumption;
        }

        public decimal? GetTotalConsumptionForDateRange(DateTime startDate, DateTime endDate)
        {
            var consumption = GetRecordsForDateRange(startDate, endDate).Sum(el => el.Delta);
            return consumption;
        }

        // Get Each Days' Consumption
        public Dictionary<DateTime, Decimal?> GetEachDaysConsumptionForDateRange(DateTime startDate, DateTime endDate)
        {
            var consumption = GetRecordsForDateRange(startDate, endDate).GroupBy(el => el.Timestamp.Date)
                                                                        .ToDictionary(g => g.Key, g => g.ToList().Sum(el => el.Delta));
            return consumption;
        }

        // Get Each Day's Records
        public Dictionary<DateTime, IEnumerable<EnergyLogs>> GetEachDaysRecordsForDateRange(DateTime startDate, DateTime endDate)
        {
            var records = GetRecordsForDateRange(startDate, endDate).GroupBy(el => el.Timestamp.Date)
                                                                    .ToDictionary(g => g.Key, g => g.AsEnumerable());
            return records;
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

        // Get last broadcase
        public EnergyLogs GetLastBroadcastFromDate(DateTime dateTime)
        {
            var record = _energyLogsRepository.Get()
                                              .OrderByDescending(el => el.Timestamp)
                                              .FirstOrDefault(el => el.Timestamp.Date == dateTime.Date);
            return record;
        }

    }
}
