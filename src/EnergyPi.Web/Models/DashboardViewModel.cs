using System;
using System.Collections.Generic;

namespace EnergyPi.Web.Models
{
    public class DashboardViewModel
    {
        // today
        public DateTime LastBroadcastTimestamp { get; set; }
        public decimal TodaysTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> TodaysHourlyConsumption { get; set; }
        public Dictionary<DateTime, Decimal> PastHourConsumption { get; set; }

        // this month
        public decimal ThisMonthsTotalConsumption { get; set; }
        public decimal ThisMonthsEstimatedTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> ThisMonthsDailyConsumption { get; set; }

    }
}
