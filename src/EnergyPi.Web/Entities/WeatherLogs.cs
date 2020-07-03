using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyPi.Web.Entities
{
    public partial class WeatherLogs : IEntity
    {
        [NotMapped]
        public int Id
        {
            get { return WeatherLogId; }
            set { WeatherLogId = value; }
        }

        public int WeatherLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? TemperatureFahrenheit { get; set; }
        public decimal? Humidity { get; set; }
    }
}
