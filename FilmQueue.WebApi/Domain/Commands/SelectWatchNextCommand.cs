using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class SelectWatchNextCommand : ICommand
    {
        public string UserId { get; set; }
        public long? FilmId { get; set; }
        public bool SelectRandomFilm { get; set; }
    }
}
