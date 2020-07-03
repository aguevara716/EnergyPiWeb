using System;

namespace EnergyPi.Web.Entities
{
    public partial class EnergyLogs
    {
        public int EnergyLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? TotalConsumption { get; set; }
        public decimal? Delta { get; set; }
    }
}
