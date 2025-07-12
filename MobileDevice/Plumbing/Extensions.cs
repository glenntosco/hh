using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pro4Soft.MobileDevice.Plumbing
{
    public static class Extensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Func<ObservableCollection<T>, IOrderedEnumerable<T>> keySelector)
        {
            var sorted = keySelector.Invoke(collection).ToList();
            var ptr = 0;
            while (ptr < sorted.Count - 1)
            {
                if (!collection[ptr].Equals(sorted[ptr]))
                {
                    var idx = collection.Search(ptr + 1, sorted[ptr]);
                    collection.Move(idx, ptr);
                }
                ptr++;
            }
        }

        public static int Search<T>(this ObservableCollection<T> collection, int startIndex, T other)
        {
            for (var i = startIndex; i < collection.Count; i++)
            {
                if (other.Equals(collection[i]))
                    return i;
            }
            return -1; // decide how to handle error case
        }
    }
}