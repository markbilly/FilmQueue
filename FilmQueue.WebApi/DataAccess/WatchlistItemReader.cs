using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface IWatchlistItemReader : IDependency
    {
        Task<IEnumerable<WatchlistItemRecord>> GetItemsByUserId(string userId, int take = 5, int skip = 0);
        Task<WatchlistItemRecord> GetById(long id);
        Task<WatchlistItemRecord> GetRandomUnwatchedItem(string userId);
        Task<WatchlistItemRecord> GetCurrentWatchNextItem(string userId);
        Task<int> GetUnwatchedItemCount(string userId);
    }

    public class WatchlistItemReader : IWatchlistItemReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public WatchlistItemReader(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WatchlistItemRecord> GetCurrentWatchNextItem(string userId)
        {
            return await _dbContext.WatchlistItemRecords
                .Where(item => item.CreatedByUserId == userId && item.WatchNextStart.HasValue && !item.WatchNextEnd.HasValue)
                .SingleOrDefaultAsync();
        }

        public Task<WatchlistItemRecord> GetById(long id)
        {
            return _dbContext.WatchlistItemRecords.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IEnumerable<WatchlistItemRecord>> GetItemsByUserId(string userId, int take = 5, int skip = 0)
        {
            return await _dbContext.WatchlistItemRecords
                .Where(item => item.CreatedByUserId.Equals(userId))
                .OrderByDescending(item => item.CreatedDateTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<WatchlistItemRecord> GetRandomUnwatchedItem(string userId)
        {
            var unwatchedItems = GetUnwatchedItems(userId);
            var count = await unwatchedItems.CountAsync();
            var randomItem = await unwatchedItems.Skip(new Random().Next(count)).FirstOrDefaultAsync();

            return randomItem;
        }

        public async Task<int> GetUnwatchedItemCount(string userId)
        {
            var unwatchedItems = GetUnwatchedItems(userId);
            return await unwatchedItems.CountAsync();
        }

        private IQueryable<WatchlistItemRecord> GetUnwatchedItems(string userId)
        {
            return _dbContext.WatchlistItemRecords
                .Where(item => item.CreatedByUserId == userId && !item.WatchedDateTime.HasValue);
        }
    }
}
