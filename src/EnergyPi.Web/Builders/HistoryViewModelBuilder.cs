using System;
using System.Collections.Generic;
using System.Linq;
using EnergyPi.Web.Entities;
using EnergyPi.Web.Models;

namespace EnergyPi.Web.Builders
{
    public interface IHistoryViewModelBuilder
    {
        HistoryViewModel BuildViewModel(DateTime date, IQueryable<EnergyLogs> energyLogs);
        HistoryViewModel BuildViewModel(DateTime date, IList<EnergyLogs> energyLogs);
    }

    public class HistoryViewModelBuilder : IHistoryViewModelBuilder
    {
        // Private Methods

        // Public Methods
        public HistoryViewModel BuildViewModel(DateTime date, IQueryable<EnergyLogs> energyLogs)
        {
            var energyLogsList = energyLogs.ToList();
            return BuildViewModel(date, energyLogsList);
        }

        public HistoryViewModel BuildViewModel(DateTime date, IList<EnergyLogs> energyLogs)
        {
            if (energyLogs == null)
                return null;

            var hvm = new HistoryViewModel();
            return hvm;
        }
    }
}
