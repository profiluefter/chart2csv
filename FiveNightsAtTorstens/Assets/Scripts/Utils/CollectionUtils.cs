using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public static class CollectionUtils
    {
        public static IEnumerable<T> Pad<T>(this IEnumerable<T> collection, int? number = null)
        {
            foreach (var value in collection)
            {
                yield return value;
                number--;
            }

            while (number > 0)
            {
                yield return default;
                number--;
            }
        }
    }
}
