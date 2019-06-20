using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Models
{
    public class WatchlistItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int RuntimeInMinutes { get; set; }
        public bool Watched { get; set; }
    }
}
