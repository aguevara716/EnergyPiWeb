using System;
using System.Collections.Generic;
using System.Linq;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Extensions;
using EnergyPi.Web.Models;

namespace EnergyPi.Web.Builders
{
    public interface IHistoryViewModelBuilder
    {
        HistoryViewModel BuildViewModel(DateTime date, IQueryable<EnergyLogs> energyLogs);
        HistoryViewModel BuildViewModel(DateTime date, IList<EnergyLogs> energyLogs);
    }

    public class HistoryViewModelBuilder : IHistoryViewModelBuilder
    {
        // Private Methods
        private Dictionary<DateTime, Decimal?> GetFocusedDaysHourlyConsumption(DateTime date, IList<EnergyLogs> energyLogs)
        {
            var hourlyConsumption = energyLogs.Where(el => el.Timestamp.Date == date.Date)
                                              .GroupBy(el => el.Timestamp.Hour)
                                              .Select(g => new { Timestamp = new DateTime(date.Year, date.Month, date.Day, g.Key, 0, 0), Consumption = g.ToList().Select(el => el.Delta).Sum() })
                                              .ToDictionary(a => a.Timestamp, a => a.Consumption);

            for (var hourIndex = 0; hourIndex < 24; hourIndex++)
            {
                var dt = new DateTime(date.Year, date.Month, date.Day, hourIndex, 0, 0);
                if (hourlyConsumption.ContainsKey(dt))
                    continue;

                hourlyConsumption[dt] = null;
            }
            hourlyConsumption = hourlyConsumption.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return hourlyConsumption;
        }

        private decimal GetFocusedDaysTotalConsumption(DateTime date, IList<EnergyLogs> energyLogs)
        {
            var totalConsumption = energyLogs.Where(el => el.Timestamp.Date == date.Date)
                                             .Sum(el => el.Delta).GetValueOrDefault();
            return totalConsumption;
        }

        private Dictionary<DateTime, Decimal> GetFocusedHourConsumption(DateTime date, IList<EnergyLogs> energyLogs)
        {
            var focusedHourConsumption = energyLogs.Where(el => el.Timestamp >= date && el.Timestamp <= date.AddHours(1))
                                                   .Select(el => new { el.Timestamp, el.Delta })
                                                   .ToDictionary(a => a.Timestamp, a => a.Delta.GetValueOrDefault());
            return focusedHourConsumption;
        }

        private decimal GetFocusedMonthsTotalConsumption(DateTime date, IList<EnergyLogs> energyLogs)
        {
            var totalConsumption = energyLogs.Sum(el => el.Delta).GetValueOrDefault();
            return totalConsumption;
        }

        private Dictionary<DateTime, Decimal?> GetFocusedMonthsDailyConsumption(DateTime date, IList<EnergyLogs> energyLogs)
        {
            var dailyConsumption = energyLogs.GroupBy(el => el.Timestamp.Date)
                                             .ToDictionary(g => g.Key, g => g.ToList().Sum(el => el.Delta));

            var lastDayNumberInMonth = DateTimeExtensions.GetLastDayOfMonth(date).Day;
            for (var dayIndex = 1; dayIndex <= lastDayNumberInMonth; dayIndex++)
            {
                var dt = new DateTime(date.Year, date.Month, dayIndex);
                if (dailyConsumption.ContainsKey(dt))
                    continue;

                dailyConsumption[dt] = null;
            }
            dailyConsumption = dailyConsumption.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return dailyConsumption;
        }

        // Public Methods
        public HistoryViewModel BuildViewModel(DateTime date, IQueryable<EnergyLogs> energyLogs)
        {
            var energyLogsList = energyLogs.ToList();
            return BuildViewModel(date, energyLogsList);
        }

        public HistoryViewModel BuildViewModel(DateTime date, IList<EnergyLogs> energyLogs)
        {
            if (energyLogs == null)
                return null;

            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            var hvm = new HistoryViewModel
            {
                FocusedDate = date,
                FocusedDaysHourlyConsumption = GetFocusedDaysHourlyConsumption(date, energyLogs),
                FocusedDaysTotalConsumption = GetFocusedDaysTotalConsumption(date, energyLogs),
                FocusedHourConsumption = GetFocusedHourConsumption(date, energyLogs),
                FocusedMonthsTotalConsumption = GetFocusedMonthsTotalConsumption(date, energyLogs),
                FocusMonthsDailyConsumption = GetFocusedMonthsDailyConsumption(date, energyLogs)
            };
            return hvm;
        }
    }
}
