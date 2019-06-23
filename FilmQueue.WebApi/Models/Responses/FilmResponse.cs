using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Models.Responses
{
    public class FilmResponse
    {
        public long Id { get; set; }
        public string Title { get; private set; }
        public string Runtime { get; private set; }

        public static FilmResponse FromDomainModel(Film watchlistItem)
        {
            return new FilmResponse
            {
                Id = watchlistItem.Id,
                Title = watchlistItem.Title,
                Runtime = watchlistItem.RuntimeInMinutes + " minutes"
            };
        }

        public static FilmResponse FromRecord(FilmRecord record)
        {
            return new FilmResponse
            {
                Id = record.Id,
                Title = record.Title,
                Runtime = record.RuntimeInMinutes + " minutes"
            };
        }
    }
}
