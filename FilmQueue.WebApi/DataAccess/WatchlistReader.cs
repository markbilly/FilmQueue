using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface IWatchlistReader : IDependency
    {
        Task<IEnumerable<WatchlistItem>> GetItemsByUserId(string userId);
    }

    public class WatchlistReader : IWatchlistReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public WatchlistReader(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WatchlistItem>> GetItemsByUserId(string userId)
        {
            return await _dbContext.WatchlistItems
                .Where(item => item.CreatedByUserId.Equals(userId))
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
