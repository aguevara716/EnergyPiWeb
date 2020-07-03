using System.ComponentModel.DataAnnotations.Schema;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Repositories;

namespace EnergyPi.Web.Entities
{
    public interface IEntity
    {
        [NotMapped]
        int Id { get; set; }
    }
}
