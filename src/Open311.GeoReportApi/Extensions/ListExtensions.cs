 // ReSharper disable once CheckNamespace
namespace Open311.GeoReportApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        public static T AddOrReplace<T, TResult>(this IList<T> list, T value) where TResult : T
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            var existing = list.OfType<TResult>().FirstOrDefault();

            if (existing == null)
            {
                list.Add(value);
            }
            else
            {
                var index = list.IndexOf(existing);
                list.Insert(index, value);
                list.Remove(existing);
            }

            return existing;
        }
    }
}
