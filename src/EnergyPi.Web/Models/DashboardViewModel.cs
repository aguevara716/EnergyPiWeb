using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnergyPi.Web.Models
{
    public class DashboardViewModel
    {
        // today
        public DateTime LastBroadcastTimestamp { get; set; }
        public decimal TodaysTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> TodaysHourlyConsumption { get; set; }
        public string TodaysHourlyConsumptionJson { get { return SerializeDictionary(TodaysHourlyConsumption); } }
        public Dictionary<DateTime, Decimal> PastHourConsumption { get; set; }
        public string PastHourConsumptionJson { get { return SerializeDictionary(PastHourConsumption); } }

        // this month
        public decimal ThisMonthsTotalConsumption { get; set; }
        public decimal ThisMonthsEstimatedTotalConsumption { get; set; }
        public Dictionary<DateTime, Decimal?> ThisMonthsDailyConsumption { get; set; }
        public string ThisMonthsDailyConsumptionJson { get { return SerializeDictionary(ThisMonthsDailyConsumption); } }

        private string SerializeDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                return String.Empty;
            return JsonConvert.SerializeObject(dictionary);
        }
    }
}
