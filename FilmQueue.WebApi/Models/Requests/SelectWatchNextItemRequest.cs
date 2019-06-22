using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Requests
{
    public class SelectWatchNextItemRequest
    {
        public long? ItemId { get; set; }
        public bool SelectRandomItem { get; set; }
    }
}
