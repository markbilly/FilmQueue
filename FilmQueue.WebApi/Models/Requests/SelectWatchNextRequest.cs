using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Requests
{
    public class SelectWatchNextRequest
    {
        public long? FilmId { get; set; }
        public bool SelectRandomFilm { get; set; }
    }
}
