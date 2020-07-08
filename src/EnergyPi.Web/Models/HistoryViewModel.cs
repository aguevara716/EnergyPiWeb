using System;
using System.Collections.Generic;

namespace EnergyPi.Web.Models
{
    public class HistoryViewModel
    {
        /*Energy Logs*/
        // Focused date
        public DateTime FocusedDate { get; set; }
        public decimal FocusedDaysTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> FocusedDaysHourlyConsumption { get; set; }
        public Dictionary<DateTime, Decimal> FocusedHourConsumption { get; set; }

        // Month
        public decimal FocusedMonthsTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> FocusMonthsDailyConsumption { get; set; }

        /*Weather Logs*/
        public decimal FocusedDaysHighestTemperature { get; set; }
        public decimal FocusedDaysAverageTemperature { get; set; }
        public decimal FocusedDaysLowestTemperature { get; set; }

        public decimal FocusedDaysHighestHumidity { get; set; }
        public decimal FocusedDaysAverageHumidity { get; set; }
        public decimal FocusedDaysLowestHumidity { get; set; }

        public decimal FocusedHoursTemperature { get; set; }
        public decimal FocusedHoursHumidity { get; set; }

        public Dictionary<DateTime, Decimal?> FocusedDaysTemperature { get; set; }
        public Dictionary<DateTime, Decimal?> FocusedMonthsHighestTemperature { get; set; }
        public Dictionary<DateTime, Decimal?> FocusedMonthsAverageTemperature { get; set; }
        public Dictionary<DateTime, Decimal?> FocusedMonthsLowestTemperature { get; set; }
    }
}
