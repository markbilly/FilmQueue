using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Events
{
    public class WatchNextItemSelectedEvent : IEvent
    {
        public long ItemId { get; set; }
        public string UserId { get; set; }
    }
}
