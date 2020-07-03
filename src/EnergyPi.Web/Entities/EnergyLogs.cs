using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyPi.Web.Entities
{
    public partial class EnergyLogs : IEntity
    {
        [NotMapped]
        public int Id
        {
            get { return EnergyLogId; }
            set { EnergyLogId = value; }
        }

        public int EnergyLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? TotalConsumption { get; set; }
        public decimal? Delta { get; set; }
    }
}
