using System;

namespace EnergyPi.Web.Entities
{
    public partial class WeatherLogs
    {
        public int WeatherLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? TemperatureFahrenheit { get; set; }
        public decimal? Humidity { get; set; }
    }
}
