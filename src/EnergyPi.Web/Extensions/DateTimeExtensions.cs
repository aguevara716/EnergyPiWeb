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

        public static DateTime GetFirstDayOfNextMonth(DateTime date)
        {
            var year = date.Year;
            var nextMonth = date.Month + 1;

            if (nextMonth == 13)
            {
                year += 1;
                nextMonth = 1;
            }

            var firstDayOfNextMonth = new DateTime(year, nextMonth, 1);
            return firstDayOfNextMonth;
        }

    }
}
