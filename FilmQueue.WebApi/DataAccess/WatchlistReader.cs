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
        Task<IEnumerable<FilmRecord>> GetWatchlist(string userId, int take = 5, int skip = 0);
    }

    public class WatchlistReader : IWatchlistReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public WatchlistReader(
            FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FilmRecord>> GetWatchlist(string userId, int take = 5, int skip = 0)
        {
            return await _dbContext.FilmRecords
                .Where(x => x.OwnedByUserId == userId && !x.WatchedDateTime.HasValue)
                .OrderByDescending(x => x.CreatedDateTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
