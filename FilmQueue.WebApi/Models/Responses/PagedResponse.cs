using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Responses
{
    public class PagedResponse<TResponseItem>
    {
        private PagedResponse(IEnumerable<TResponseItem> items, int page, int pageSize, int totalItems)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        }

        public IEnumerable<TResponseItem> Items { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public static PagedResponse<TResponseItem> FromEnumerable(IEnumerable<TResponseItem> items, int page, int pageSize, int totalItems)
        {
            return new PagedResponse<TResponseItem>(items, page, pageSize, totalItems);
        }

        public static PagedResponse<TResponseItem> FromEnumerable<TEnumerableItem>(IEnumerable<TEnumerableItem> items, Func<TEnumerableItem, TResponseItem> propertySelector, int page, int pageSize, int totalItems)
        {
            return new PagedResponse<TResponseItem>(items.Select(propertySelector), page, pageSize, totalItems);
        }
    }
}
