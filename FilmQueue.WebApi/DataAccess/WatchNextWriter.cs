using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface IWatchNextWriter : IDependency
    {
        Task MakeSelection(long filmId, string userId);
        Task ExpireSelection(long filmId, bool watched = true);
    }

    public class WatchNextWriter : IWatchNextWriter
    {
        private readonly FilmQueueDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IClock _clock;

        public WatchNextWriter(
            FilmQueueDbContext dbContext,
            IMemoryCache memoryCache,
            IClock clock)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _clock = clock;
        }

        public async Task ExpireSelection(long filmId, bool watched = true)
        {
            var selectionRecord = await _dbContext.WatchNextSelectionRecords
                .Where(x => x.FilmId == filmId && !x.ExpiredDateTime.HasValue)
                .SingleOrDefaultAsync();

            selectionRecord.ExpiredDateTime = _clock.UtcNow;
            selectionRecord.Watched = watched ? true : false;

            await _dbContext.SaveChangesAsync();

            _memoryCache.Remove(CacheKeys.WatchNext(selectionRecord.UserId));
        }

        public async Task MakeSelection(long filmId, string userId)
        {
            await _dbContext.WatchNextSelectionRecords.AddAsync(new WatchNextSelectionRecord
            {
                FilmId = filmId,
                UserId = userId,
                SelectedDateTime = _clock.UtcNow
            });

            await _dbContext.SaveChangesAsync();

            _memoryCache.Remove(CacheKeys.WatchNext(userId));
        }
    }
}
