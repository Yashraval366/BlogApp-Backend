using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Helpers
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; } 
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPrev => PageIndex > 1;
        public bool HasNext => PageIndex < TotalCount;


        private PaginatedList(List<T> items,  int pageIndex, int pageSize, int count)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<T>(items, pageIndex, pageSize, count);   
        }

    }
}
