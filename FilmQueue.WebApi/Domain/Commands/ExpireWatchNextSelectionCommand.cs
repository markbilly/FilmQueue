using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class ExpireWatchNextSelectionCommand : ICommand
    {
        public long FilmId { get; set; }
        public WatchNextExpiryReason Reason { get; set; }
    }
}
