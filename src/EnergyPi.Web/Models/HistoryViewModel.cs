using System;
using System.Collections.Generic;

namespace EnergyPi.Web.Models
{
    public class HistoryViewModel
    {
        // Focused date
        public DateTime FocusedDate { get; set; }
        public decimal FocusedDaysTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> FocusedDaysHourlyConsumption { get; set; }
        public Dictionary<DateTime, Decimal> FocusedHourConsumption { get; set; }

        // Month
        public decimal FocusedMonthsTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> FocusMonthsDailyConsumption { get; set; }
    }
}
