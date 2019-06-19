using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Events
{
    public class WatchlistItemCreatedEvent : IEvent
    {
        public long ItemId { get; set; }
        public WatchlistItem Item { get; set; }
    }
}
