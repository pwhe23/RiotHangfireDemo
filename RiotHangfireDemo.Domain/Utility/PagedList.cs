using System.Linq;

namespace RiotHangfireDemo.Domain
{
    public interface IPageable
    {
        int? PageNumber { get; set; }
        int? PageSize { get; set; }
    };

    public class PagedList<T>
    {
        public T[] Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    };

    public static class PagedList
    {
        public static PagedList<T> ToPagedList<T>(this IOrderedQueryable<T> query, IPageable cmd)
        {
            var list = new PagedList<T>
            {
                PageNumber = !cmd.PageNumber.HasValue || cmd.PageNumber < 1
                    ? 1
                    : cmd.PageNumber.Value,
                PageSize = !cmd.PageSize.HasValue || cmd.PageSize < 1 || cmd.PageSize > 1000
                    ? 10
                    : cmd.PageSize.Value,
            };

            var recordsToSkip = list.PageNumber > 1
                ? (list.PageNumber - 1) * list.PageSize
                : 0;

            var result = query
                .Skip(recordsToSkip)
                .Take(list.PageSize)
                .GroupBy(x => new { Total = query.Count() })
                .FirstOrDefault();

            list.Items = result?.ToArray() ?? new T[0];
            list.TotalItems = result?.Key.Total ?? 0;

            return list;
        }
    };
}
