using System;
using System.Collections.Generic;

namespace EnergyPi.Web.Models
{
    public class TodayViewModel
    {
        /*Energy Logs*/
        // today
        public DateTime LastBroadcastTimestamp { get; set; }
        public decimal TodaysTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> TodaysHourlyConsumption { get; set; }
        public Dictionary<DateTime, Decimal> PastHourConsumption { get; set; }
        public decimal LatestPowerDraw { get; set; }
        public Dictionary<DateTime, Decimal?> PastHourPowerDraw { get; set; }

        // this month
        public decimal ThisMonthsTotalConsumption { get; set; }
        public decimal ThisMonthsEstimatedTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> ThisMonthsDailyConsumption { get; set; }

        /*Weather Logs*/
        public decimal LastTemperatureReading { get; set; }
        public decimal LastHumidityReading { get; set; }
        public Dictionary<DateTime, Decimal?> TodaysTemperatureReadings { get; set; }
        public Dictionary<DateTime, Decimal?> ThisMonthsDailyAverageTemperatureReadings { get; set; }
        public Dictionary<DateTime, Decimal?> ThisMonthsDailyHighTemperatureReadings { get; set; }
        public Dictionary<DateTime, Decimal?> ThisMonthsDailyLowTemperatureReadings { get; set; }
    }
}
