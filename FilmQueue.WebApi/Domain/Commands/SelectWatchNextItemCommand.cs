using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class SelectWatchNextItemCommand : ICommand
    {
        public string UserId { get; set; }
        public long? ItemId { get; set; }
        public bool SelectRandomItem { get; set; }
    }
}
