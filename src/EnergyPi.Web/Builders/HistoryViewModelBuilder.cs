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
        HistoryViewModel BuildViewModel(DateTime date, IQueryable<EnergyLogs> energyLogs, IQueryable<WeatherLogs> weatherLogs);
        HistoryViewModel BuildViewModel(DateTime date, IList<EnergyLogs> energyLogs, IList<WeatherLogs> weatherLogs);
    }

    public class HistoryViewModelBuilder : IHistoryViewModelBuilder
    {
        // EnergyLogs Methods
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

        // WeatherLogs Methods
        private decimal GetFocusedDaysAverageHumidity(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var averageHumidity = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                             .Average(wl => wl.Humidity)
                                             .GetValueOrDefault();
            return averageHumidity;
        }

        private decimal GetFocusedDaysAverageTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var averageTemperature = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                                .Average(wl => wl.TemperatureFahrenheit)
                                                .GetValueOrDefault();
            return averageTemperature;
        }

        private Dictionary<DateTime, decimal?> GetFocusedMonthsAverageTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.GroupBy(wl => wl.Timestamp.Date)
                                     .Select(g => new { Timestamp = g.Key, Temperature = g.ToList().Average(wl => wl.TemperatureFahrenheit) })
                                     .ToDictionary(a => a.Timestamp, a => a.Temperature);
            return results;
        }

        private decimal GetFocusedDaysHighestHumidity(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var highestHumidity = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                             .Max(wl => wl.Humidity)
                                             .GetValueOrDefault();
            return highestHumidity;
        }

        private decimal GetFocusedDaysHighestTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var highestTemperature = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                                .Max(wl => wl.TemperatureFahrenheit)
                                                .GetValueOrDefault();
            return highestTemperature;
        }

        private decimal GetFocusedDaysLowestHumidity(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var lowestHumidity = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                            .Min(wl => wl.Humidity)
                                            .GetValueOrDefault();
            return lowestHumidity;
        }

        private decimal GetFocusedDaysLowestTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var lowestTemperature = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                               .Min(wl => wl.TemperatureFahrenheit)
                                               .GetValueOrDefault();
            return lowestTemperature;
        }

        private Dictionary<DateTime, decimal?> GetFocusedDaysTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.Where(wl => wl.Timestamp.Date == date.Date)
                                     .ToDictionary(wl => wl.Timestamp, wl => wl.TemperatureFahrenheit);
            return results;
        }

        private decimal GetFocusedHoursHumidity(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var humidity = weatherLogs.FirstOrDefault(wl => wl.Timestamp.Date == date.Date &&
                                                            wl.Timestamp.Hour == date.Hour)
                                      .Humidity
                                      .GetValueOrDefault();
            return humidity;
        }

        private decimal GetFocusedHoursTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var temperature = weatherLogs.FirstOrDefault(wl => wl.Timestamp.Date == date.Date &&
                                                               wl.Timestamp.Hour == date.Hour)
                                         .TemperatureFahrenheit
                                         .GetValueOrDefault();
            return temperature;
        }

        private Dictionary<DateTime, decimal?> GetFocusedMonthsHighestTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.GroupBy(wl => wl.Timestamp.Date)
                                     .Select(g => new { Timestamp = g.Key, Temperature = g.ToList().Max(wl => wl.TemperatureFahrenheit) })
                                     .ToDictionary(a => a.Timestamp, a => a.Temperature);
            return results;
        }

        private Dictionary<DateTime, decimal?> GetFocusedMonthsLowestTemperature(DateTime date, IList<WeatherLogs> weatherLogs)
        {
            var results = weatherLogs.GroupBy(wl => wl.Timestamp.Date)
                                     .Select(g => new { Timestamp = g.Key, Temperature = g.ToList().Min(wl => wl.TemperatureFahrenheit) })
                                     .ToDictionary(a => a.Timestamp, a => a.Temperature);
            return results;
        }

        // Public Methods
        public HistoryViewModel BuildViewModel(DateTime date, IQueryable<EnergyLogs> energyLogs, IQueryable<WeatherLogs> weatherLogs)
        {
            var energyLogsList = energyLogs.ToList();
            var weatherLogsList = weatherLogs.ToList();

            return BuildViewModel(date, energyLogsList, weatherLogsList);
        }

        public HistoryViewModel BuildViewModel(DateTime date, IList<EnergyLogs> energyLogs, IList<WeatherLogs> weatherLogs)
        {
            if (energyLogs == null)
                return null;

            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            var hvm = new HistoryViewModel
            {
                // EnergyLogs
                FocusedDate = date,
                FocusedDaysHourlyConsumption = GetFocusedDaysHourlyConsumption(date, energyLogs),
                FocusedDaysTotalConsumption = GetFocusedDaysTotalConsumption(date, energyLogs),
                FocusedHourConsumption = GetFocusedHourConsumption(date, energyLogs),
                FocusedMonthsTotalConsumption = GetFocusedMonthsTotalConsumption(date, energyLogs),
                FocusMonthsDailyConsumption = GetFocusedMonthsDailyConsumption(date, energyLogs),

                // WeatherLogs
                FocusedDaysAverageHumidity = GetFocusedDaysAverageHumidity(date, weatherLogs),
                FocusedDaysAverageTemperature = GetFocusedDaysAverageTemperature(date, weatherLogs),
                FocusedMonthsAverageTemperature = GetFocusedMonthsAverageTemperature(date, weatherLogs),
                FocusedDaysHighestHumidity = GetFocusedDaysHighestHumidity(date, weatherLogs),
                FocusedDaysHighestTemperature = GetFocusedDaysHighestTemperature(date, weatherLogs),
                FocusedDaysLowestHumidity = GetFocusedDaysLowestHumidity(date, weatherLogs),
                FocusedDaysLowestTemperature = GetFocusedDaysLowestTemperature(date, weatherLogs),
                FocusedDaysTemperature = GetFocusedDaysTemperature(date, weatherLogs),
                FocusedHoursHumidity = GetFocusedHoursHumidity(date, weatherLogs),
                FocusedHoursTemperature = GetFocusedHoursTemperature(date, weatherLogs),
                FocusedMonthsHighestTemperature = GetFocusedMonthsHighestTemperature(date, weatherLogs),
                FocusedMonthsLowestTemperature = GetFocusedMonthsLowestTemperature(date, weatherLogs)
            };
            return hvm;
        }

    }
}
