﻿using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Events
{
    public class WatchlistItemUpdateFailedEvent : IEvent
    {
        public bool ItemNotFound { get; set; }
        public IDictionary<string, string> ValidationMessages { get; set; } = new Dictionary<string, string>();
    }
}