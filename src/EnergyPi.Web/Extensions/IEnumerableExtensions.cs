using System.Collections.Generic;
using System.Linq;

namespace EnergyPi.Web.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            var isNullOrEmpty = enumerable == null || !enumerable.Any();
            return isNullOrEmpty;
        }

    }
}
