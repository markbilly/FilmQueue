using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain
{
    public class AddToWatchlistRequest
    {
        public string Title { get; set; }
        public int RuntimeInMinutes { get; set; }
    }
}
