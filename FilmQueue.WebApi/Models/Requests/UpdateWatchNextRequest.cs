using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Requests
{
    public class UpdateWatchNextRequest
    {
        public bool IsWatched { get; set; }
    }
}
