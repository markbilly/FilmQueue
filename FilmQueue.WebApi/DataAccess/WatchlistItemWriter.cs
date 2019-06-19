using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface IWatchlistItemWriter : IDependency
    {
        Task<WatchlistItem> Create(string userId, string title, int runtimeInMinutes);
    }

    public class WatchlistItemWriter : IWatchlistItemWriter
    {
        private readonly FilmQueueDbContext _dbContext;
        private readonly IClock _clock;

        public WatchlistItemWriter(
            FilmQueueDbContext dbContext,
            IClock clock)
        {
            _dbContext = dbContext;
            _clock = clock;
        }

        public async Task<WatchlistItem> Create(string userId, string title, int runtimeInMinutes)
        {
            var created = await _dbContext.AddAsync(new WatchlistItem
            {
                Title = title,
                RuntimeInMinutes = runtimeInMinutes,
                CreatedDateTime = _clock.UtcNow,
                CreatedByUserId = userId
            });

            return created.Entity;
        }
    }
}
