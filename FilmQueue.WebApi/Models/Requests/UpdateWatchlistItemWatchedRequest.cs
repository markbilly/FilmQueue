using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Requests
{
    public class UpdateWatchlistItemWatchedRequest
    {
        public long ItemId { get; set; }
        public bool Watched { get; set; }
    }
}
