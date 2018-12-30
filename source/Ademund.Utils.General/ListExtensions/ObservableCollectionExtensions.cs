using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ademund.Utils.ListExtensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            var sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count; i++)
                collection.Move(collection.IndexOf(sorted[i]), i);
        }

        public static void AddRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        public static void InsertRange<TSource>(this ObservableCollection<TSource> source, int index, IEnumerable<TSource> items)
        {
            foreach (var item in items.Reverse())
            {
                source.Insert(index, item);
            }
        }
    }
}