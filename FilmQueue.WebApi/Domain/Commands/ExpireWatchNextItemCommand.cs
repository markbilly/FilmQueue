using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class ExpireWatchNextItemCommand : ICommand
    {
        public long ItemId { get; set; }
    }
}
