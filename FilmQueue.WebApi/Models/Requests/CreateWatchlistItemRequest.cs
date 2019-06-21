using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Requests
{
    public class CreateToWatchlistItemRequest
    {
        public string Title { get; set; }
        public int RuntimeInMinutes { get; set; }
    }
}
