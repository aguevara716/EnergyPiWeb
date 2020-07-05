using System;

namespace EnergyPi.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfMonth;
        }

        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            var firstDayOfNextMonth = GetFirstDayOfNextMonth(date);
            var lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            return lastDayOfMonth;
        }

        public static DateTime GetFirstDayOfNextMonth(DateTime date)
        {
            var dateNextMonth = date.AddMonths(1);
            var firstDayOfNextMonth = new DateTime(dateNextMonth.Year, dateNextMonth.Month, 1);
            return firstDayOfNextMonth;
        }

    }
}
