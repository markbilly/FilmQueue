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
    public interface IWatchNextReader : IDependency
    {
        Task<FilmRecord> GetActiveWatchNext(string userId);
        Task<FilmRecord> GetMostRecentWatchNext(string userId);
        Task<bool> IsFilmWatchNextSelection(long filmId);
    }

    public class WatchNextReader : IWatchNextReader
    {
        private readonly FilmQueueDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public WatchNextReader(
            FilmQueueDbContext dbContext,
            IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<FilmRecord> GetActiveWatchNext(string userId)
        {
            return await _memoryCache.GetOrCreateAsync(CacheKeys.WatchNext(userId), async entry =>
            {
                var selectionRecord = await _dbContext.WatchNextSelectionRecords
                    .Where(x => x.UserId == userId && !x.ExpiredDateTime.HasValue)
                    .SingleOrDefaultAsync();

                if (selectionRecord == null)
                {
                    return null;
                }

                return await _dbContext.FilmRecords
                    .Where(item => item.Id == selectionRecord.FilmId)
                    .SingleOrDefaultAsync();
            });
        }

        public async Task<FilmRecord> GetMostRecentWatchNext(string userId)
        {
            var selectionRecord = await _dbContext.WatchNextSelectionRecords
                .Where(x => x.UserId == userId && x.ExpiredDateTime.HasValue)
                .OrderByDescending(x => x.ExpiredDateTime.Value)
                .FirstOrDefaultAsync();

            if (selectionRecord == null)
            {
                return null;
            }

            return await _dbContext.FilmRecords.SingleOrDefaultAsync(x => x.Id == selectionRecord.FilmId);
        }

        public Task<bool> IsFilmWatchNextSelection(long filmId)
        {
            return _dbContext.WatchNextSelectionRecords.AnyAsync(x => x.FilmId == filmId && !x.ExpiredDateTime.HasValue);
        }
    }
}
