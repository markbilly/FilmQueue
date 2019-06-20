using FilmQueue.WebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Responses
{
    public class WatchlistItemResponse
    {
        public WatchlistItemResponse(WatchlistItem watchlistItem)
        {
            Id = watchlistItem.Id;
            Title = watchlistItem.Title;
            Runtime = watchlistItem.RuntimeInMinutes + " minutes";
        }

        public long Id { get; set; }
        public string Title { get; private set; }
        public string Runtime { get; private set; }
    }
}
