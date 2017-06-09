using System.Collections.Generic;
using System.Linq;

namespace RiotHangfireDemo
{
    public interface IPageable
    {
        int? PageNumber { get; set; }
        int? PageSize { get; set; }
    };

    public class PagedList<T>
    {
        public PagedList(IEnumerable<T> items, int pageNumber, int totalItems)
        {
            Items = items ?? new T[0];
            PageNumber = pageNumber;
            TotalItems = totalItems;
        }

        public IEnumerable<T> Items { get; }
        public int PageNumber { get; }
        public int TotalItems { get; }
    };

    public static class PagedList
    {
        public static PagedList<T> ToPagedList<T>(this IOrderedQueryable<T> query, IPageable cmd)
        {
            if (!cmd.PageNumber.HasValue || cmd.PageNumber < 1)
                cmd.PageNumber = 1;

            if (!cmd.PageSize.HasValue)
                cmd.PageSize = 10;

            var recordsToSkip = cmd.PageNumber > 1 ? (cmd.PageNumber.Value - 1) * cmd.PageSize.Value : 0;

            var result = query
                .Skip(recordsToSkip)
                .Take(cmd.PageSize.Value)
                .GroupBy(x => new { Total = query.Count() })
                .FirstOrDefault();

            return new PagedList<T>(result, cmd.PageNumber.Value, result?.Key.Total ?? 0);
        }
    };
}
