using FilmQueue.WebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Models
{
    public class Film
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int RuntimeInMinutes { get; set; }
        public bool Watched { get; set; }

        public static Film FromRecord(FilmRecord record)
        {
            return new Film
            {
                Id = record.Id,
                Title = record.Title,
                RuntimeInMinutes = record.RuntimeInMinutes,
                Watched = record.WatchedDateTime.HasValue
            };
        }
    }
}
