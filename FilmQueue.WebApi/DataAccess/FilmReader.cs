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
    public interface IFilmReader : IDependency
    {
        Task<FilmRecord> GetFilmById(long filmId);
        Task<FilmRecord> GetRandomUnwatchedFilm(string userId);
        Task<int> GetUnwatchedFilmCount(string userId);

        // Move to WatchNextReader
        Task<FilmRecord> GetCurrentWatchNextItem(string userId);

        // Move to WatchlistReader
        Task<IEnumerable<FilmRecord>> GetItemsByUserId(string userId, int take = 5, int skip = 0);
    }

    public class FilmReader : IFilmReader
    {
        private readonly FilmQueueDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public FilmReader(
            FilmQueueDbContext dbContext,
            IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<FilmRecord> GetCurrentWatchNextItem(string userId)
        {
            return await _memoryCache.GetOrCreateAsync(CacheKeys.WatchNext(userId), entry =>
            {
                return _dbContext.WatchlistItemRecords
                    .Where(item => item.CreatedByUserId == userId && item.WatchNextStart.HasValue && !item.WatchNextEnd.HasValue)
                    .SingleOrDefaultAsync();
            });
        }

        public Task<FilmRecord> GetFilmById(long filmId)
        {
            return _dbContext.WatchlistItemRecords.FirstOrDefaultAsync(item => item.Id == filmId);
        }

        public async Task<IEnumerable<FilmRecord>> GetItemsByUserId(string userId, int take = 5, int skip = 0)
        {
            return await _dbContext.WatchlistItemRecords
                .Where(item => item.CreatedByUserId.Equals(userId))
                .OrderByDescending(item => item.CreatedDateTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<FilmRecord> GetRandomUnwatchedFilm(string userId)
        {
            var unwatchedItems = GetUnwatchedItems(userId);
            var count = await unwatchedItems.CountAsync();
            var randomItem = await unwatchedItems.Skip(new Random().Next(count)).FirstOrDefaultAsync();

            return randomItem;
        }

        public async Task<int> GetUnwatchedFilmCount(string userId)
        {
            var unwatchedItems = GetUnwatchedItems(userId);
            return await unwatchedItems.CountAsync();
        }

        private IQueryable<FilmRecord> GetUnwatchedItems(string userId)
        {
            return _dbContext.WatchlistItemRecords
                .Where(item => item.CreatedByUserId == userId && !item.WatchedDateTime.HasValue);
        }
    }
}
