using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Responses
{
    public class WatchlistItemResponse
    {
        public long Id { get; set; }
        public string Title { get; private set; }
        public string Runtime { get; private set; }

        public static WatchlistItemResponse FromDomainModel(WatchlistItem watchlistItem)
        {
            return new WatchlistItemResponse
            {
                Id = watchlistItem.Id,
                Title = watchlistItem.Title,
                Runtime = watchlistItem.RuntimeInMinutes + " minutes"
            };
        }

        public static WatchlistItemResponse FromRecord(WatchlistItemRecord record)
        {
            return new WatchlistItemResponse
            {
                Id = record.Id,
                Title = record.Title,
                Runtime = record.RuntimeInMinutes + " minutes"
            };
        }
    }
}
