using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Events
{
    public class WatchNextItemUpdateFailedEvent : IEvent
    {
        public bool NoWatchNextItemFound { get; set; }
        public long? ItemId { get; set; }
        public IDictionary<string, string> ValidationMessages { get; set; } = new Dictionary<string, string>();
    }
}
