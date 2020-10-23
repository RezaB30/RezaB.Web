using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Extentions
{
    public static class SplitArray
    {
        public static IEnumerable<IEnumerable<T>> Chuncks<T>(this IEnumerable<T> list, int chunkSize)
        {
            for (int i = 0; i < list.Count(); i += chunkSize)
            {
                yield return list.Skip(i).Take(chunkSize);
            }
        }
    }
}