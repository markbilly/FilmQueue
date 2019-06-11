using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.Domain
{
    public class QueryResponse<TResponseItem>
    {
        private QueryResponse(IEnumerable<TResponseItem> items)
        {
            Items = items;
        }

        public IEnumerable<TResponseItem> Items { get; private set; }

        public static QueryResponse<TResponseItem> FromEnumerable(IEnumerable<TResponseItem> items)
        {
            return new QueryResponse<TResponseItem>(items);
        }

        public static QueryResponse<TResponseItem> FromEnumerable<TEnumerableItem>(IEnumerable<TEnumerableItem> items, Func<TEnumerableItem, TResponseItem> propertySelector)
        {
            return new QueryResponse<TResponseItem>(items.Select(propertySelector));
        }
    }
}
