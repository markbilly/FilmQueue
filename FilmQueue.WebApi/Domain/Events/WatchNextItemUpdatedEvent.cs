using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Events
{
    public class WatchNextItemUpdatedEvent : IEvent
    {
        public WatchlistItem Item { get; set; }
    }
}
