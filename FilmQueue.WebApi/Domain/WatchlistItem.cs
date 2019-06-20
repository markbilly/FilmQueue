using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain
{
    public class WatchlistItem
    {
        public WatchlistItem(string title, int runtimeInMinutes)
        {
            Title = title;
            Runtime = runtimeInMinutes + " minutes";
        }

        public string Title { get; private set; }
        public string Runtime { get; private set; }
    }
}
