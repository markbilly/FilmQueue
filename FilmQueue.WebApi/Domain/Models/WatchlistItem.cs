using FilmQueue.WebApi.DataAccess.Models;
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

        public static WatchlistItem FromRecord(FilmRecord record)
        {
            return new WatchlistItem
            {
                Id = record.Id,
                Title = record.Title,
                RuntimeInMinutes = record.RuntimeInMinutes,
                Watched = record.WatchedDateTime.HasValue
            };
        }
    }
}
