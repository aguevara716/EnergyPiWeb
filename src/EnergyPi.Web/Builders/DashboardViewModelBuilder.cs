using System;
using System.Collections.Generic;
using System.Linq;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Extensions;
using EnergyPi.Web.Models;

namespace EnergyPi.Web.Builders
{
    public interface IDashboardViewModelBuilder
    {
        DashboardViewModel BuildDashboardViewModel(IQueryable<EnergyLogs> energyLogs, IQueryable<WeatherLogs> weatherLogs);
        DashboardViewModel BuildDashboardViewModel(IList<EnergyLogs> energyLogs, IList<WeatherLogs> weatherLogs);
    }

    public class DashboardViewModelBuilder : IDashboardViewModelBuilder
    {
        // EnergyLogs Methods
        private DateTime GetLastBroadcastTimestamp(IList<EnergyLogs> energyLogs)
        {
            var lastBroadcast = energyLogs.OrderByDescending(el => el.Timestamp).FirstOrDefault();
            return lastBroadcast.Timestamp;
        }

        private Dictionary<DateTime, Decimal> GetPastHourConsumption(IList<EnergyLogs> energyLogs)
        {
            var pastHourConsumption = energyLogs.Where(el => el.Timestamp < DateTime.Now && el.Timestamp >= DateTime.Now.AddHours(-1))
                                                .Select(el => new { el.Timestamp, el.Delta })
                                                .ToDictionary(a => a.Timestamp, a => a.Delta.GetValueOrDefault());
            return pastHourConsumption;
        }

        private Dictionary<DateTime, Decimal?> GetThisMonthsDailyConsumption(IList<EnergyLogs> energyLogs)
        {
            var dailyConsumption = energyLogs.GroupBy(el => el.Timestamp.Date)
                                             .ToDictionary(g => g.Key, g => g.ToList().Sum(el => el.Delta));
            var nextDay = DateTime.Now.AddDays(1);
            while (nextDay.Month == DateTime.Now.Month)
            {
                dailyConsumption[nextDay.Date] = null;
                nextDay = nextDay.AddDays(1);
            }
            return dailyConsumption;
        }

        private decimal GetThisMonthsEstimatedConsumption(IList<EnergyLogs> energyLogs)
        {
            var latestTimestamp = energyLogs.OrderByDescending(el => el.Timestamp).FirstOrDefault().Timestamp;

            var daysIntoMonth = (decimal)(latestTimestamp - DateTimeExtensions.GetFirstDayOfMonth(latestTimestamp)).TotalDays;
            var remainingDays = (decimal)(DateTimeExtensions.GetFirstDayOfNextMonth(latestTimestamp) - DateTimeExtensions.GetFirstDayOfMonth(latestTimestamp)).TotalDays;

            var avgDailyConsumption = energyLogs.Where(el => el.Timestamp.Month == DateTime.Now.Month).Sum(el => el.Delta).GetValueOrDefault() / daysIntoMonth;

            var estimatedConsumption = avgDailyConsumption * remainingDays;

            return estimatedConsumption;
        }

        private decimal GetThisMonthsTotalConsumption(IList<EnergyLogs> energyLogs)
        {
            var totalConsumption = energyLogs.Sum(el => el.Delta).GetValueOrDefault();
            return totalConsumption;
        }

        private Dictionary<DateTime, Decimal?> GetTodaysHourlyConsumption(IList<EnergyLogs> energyLogs)
        {
            var now = DateTime.Now;
            var totalConsumption = energyLogs.Where(el => el.Timestamp.Date == DateTime.Now.Date)
                                             .GroupBy(el => el.Timestamp.Hour)
                                             .Select(g => new { Timestamp = new DateTime(now.Year, now.Month, now.Day, g.Key, 0, 0), Consumption = g.ToList().Select(el => el.Delta).Sum() })
                                             .ToDictionary(a => a.Timestamp, a => a.Consumption);
            var nextHour = now.AddHours(1);
            while (nextHour.Day == DateTime.Now.Day)
            {
                var key = new DateTime(nextHour.Year, nextHour.Month, nextHour.Day, nextHour.Hour, 0, 0);
                totalConsumption[key] = null;
                nextHour = nextHour.AddHours(1);
            }
            return totalConsumption;
        }

        private decimal GetTodaysTotalConsumption(IList<EnergyLogs> energyLogs)
        {
            var totalConsumption = energyLogs.Where(el => el.Timestamp.Date == DateTime.Now.Date)
                                             .Sum(el => el.Delta)
                                             .GetValueOrDefault();
            return totalConsumption;
        }

        // WeatherLogs Methods
        private decimal GetLastHumidityReading(IList<WeatherLogs> weatherLogs)
        {
            var lastHumidityReading = weatherLogs.OrderByDescending(wl => wl.Timestamp)
                                                 .FirstOrDefault()
                                                 .Humidity
                                                 .GetValueOrDefault();
            return lastHumidityReading;
        }

        private decimal GetLastTemperatureReading(IList<WeatherLogs> weatherLogs)
        {
            var lastTemperatureReading = weatherLogs.OrderByDescending(wl => wl.Timestamp)
                                                    .FirstOrDefault()
                                                    .TemperatureFahrenheit
                                                    .GetValueOrDefault();
            return lastTemperatureReading;
        }

        private Dictionary<DateTime, Decimal?> GetThisMonthsDailyAverageTemperatureReadings(IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.GroupBy(wl => wl.Timestamp.Date)
                                     .Select(g => new { Timestamp = g.Key, HighestTemperature = g.ToList().Average(wl => wl.TemperatureFahrenheit) })
                                     .ToDictionary(a => a.Timestamp, a => a.HighestTemperature);
            return results;
        }

        private Dictionary<DateTime, Decimal?> GetThisMonthsDailyHighTemperatureReadings(IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.GroupBy(wl => wl.Timestamp.Date)
                                     .Select(g => new { Timestamp = g.Key, HighestTemperature = g.ToList().Max(wl => wl.TemperatureFahrenheit) })
                                     .ToDictionary(a => a.Timestamp, a => a.HighestTemperature);
            return results;
        }

        private Dictionary<DateTime, Decimal?> GetThisMonthsDailyLowTemperatureReadings(IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.GroupBy(wl => wl.Timestamp.Date)
                                     .Select(g => new { Timestamp = g.Key, HighestTemperature = g.ToList().Min(wl => wl.TemperatureFahrenheit) })
                                     .ToDictionary(a => a.Timestamp, a => a.HighestTemperature);
            return results;
        }

        private Dictionary<DateTime, Decimal?> GetTodaysWeatherReadings(IList<WeatherLogs> weatherLogs)
        {
            var records = weatherLogs.Where(wl => wl.Timestamp.Date == DateTime.Now.Date)
                                     .ToDictionary(wl => wl.Timestamp, wl => wl.TemperatureFahrenheit);
            return records;
        }

        // Public Methods
        public DashboardViewModel BuildDashboardViewModel(IQueryable<EnergyLogs> energyLogs, IQueryable<WeatherLogs> weatherLogs)
        {
            var energyLogsList = energyLogs.ToList();
            var weatherLogsList = weatherLogs.ToList();
            return BuildDashboardViewModel(energyLogsList, weatherLogsList);
        }

        public DashboardViewModel BuildDashboardViewModel(IList<EnergyLogs> energyLogs, IList<WeatherLogs> weatherLogs)
        {
            if (energyLogs.IsNullOrEmpty())
                return null;

            var dvm = new DashboardViewModel
            {
                // Energy Logs
                LastBroadcastTimestamp = GetLastBroadcastTimestamp(energyLogs),
                PastHourConsumption = GetPastHourConsumption(energyLogs),
                ThisMonthsDailyConsumption = GetThisMonthsDailyConsumption(energyLogs),
                ThisMonthsEstimatedTotalConsumption = GetThisMonthsEstimatedConsumption(energyLogs),
                ThisMonthsTotalConsumption = GetThisMonthsTotalConsumption(energyLogs),
                TodaysHourlyConsumption = GetTodaysHourlyConsumption(energyLogs),
                TodaysTotalConsumption = GetTodaysTotalConsumption(energyLogs),
                // Weather Logs
                LastHumidityReading = GetLastHumidityReading(weatherLogs),
                LastTemperatureReading = GetLastTemperatureReading(weatherLogs),
                ThisMonthsDailyAverageTemperatureReadings = GetThisMonthsDailyAverageTemperatureReadings(weatherLogs),
                ThisMonthsDailyHighTemperatureReadings = GetThisMonthsDailyHighTemperatureReadings(weatherLogs),
                ThisMonthsDailyLowTemperatureReadings = GetThisMonthsDailyLowTemperatureReadings(weatherLogs),
                TodaysTemperatureReadings = GetTodaysWeatherReadings(weatherLogs)
            };

            return dvm;
        }

    }
}
