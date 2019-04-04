using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmina.Web.Extension
{
    public static class ListExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void AddOrUpdate(this Dictionary<int, string> dic, int key, string newValue)
        {
            if (dic.TryGetValue(key, out string val))
            {
                // value exists!
                dic[key] = val + newValue;
            }
            else
            {
                //add the new value
                dic.Add(key, newValue);
            }
        }

    }
}
