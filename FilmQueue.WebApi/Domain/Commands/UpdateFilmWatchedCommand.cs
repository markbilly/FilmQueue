using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class UpdateFilmWatchedCommand : ICommand
    {
        public long ItemId { get; set; }
        public bool Watched { get; set; }
    }
}
