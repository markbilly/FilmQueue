using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class UpdateFilmCommand : ICommand
    {
        public long FilmId { get; set; }
        public string Title { get; set; }
        public int RuntimeInMinutes { get; set; }
    }
}
