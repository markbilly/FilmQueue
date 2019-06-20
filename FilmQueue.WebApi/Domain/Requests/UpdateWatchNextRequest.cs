using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Requests
{
    public class UpdateWatchNextRequest
    {
        public bool IsWatched { get; set; }
    }
}
