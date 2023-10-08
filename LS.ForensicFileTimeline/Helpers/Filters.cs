using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.ForensicFileTimeline.Helpers
{
    public static class Filters
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>  (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
