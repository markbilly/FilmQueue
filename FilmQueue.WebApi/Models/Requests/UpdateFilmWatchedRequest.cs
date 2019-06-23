using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Requests
{
    public class UpdateFilmWatchedRequest
    {
        public bool Watched { get; set; }
    }
}
