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
        Task<IEnumerable<WatchlistItem>> GetItemsByUserId(string userId, int take = 5, int skip = 0);
        Task<WatchlistItem> GetItemById(long id);
        Task<WatchlistItem> GetRandomUnwatchedItem(string userId);
        Task<WatchlistItem> GetCurrentWatchNextItem(string userId);
        Task<int> GetUnwatchedItemCount(string userId);
    }

    public class WatchlistReader : IWatchlistReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public WatchlistReader(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WatchlistItem> GetCurrentWatchNextItem(string userId)
        {
            return await _dbContext.WatchlistItems
                .Where(item => item.CreatedByUserId == userId && item.WatchNextStart.HasValue && !item.WatchNextEnd.HasValue)
                .SingleOrDefaultAsync();
        }

        public Task<WatchlistItem> GetItemById(long id)
        {
            return _dbContext.WatchlistItems.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IEnumerable<WatchlistItem>> GetItemsByUserId(string userId, int take = 5, int skip = 0)
        {
            return await _dbContext.WatchlistItems
                .Where(item => item.CreatedByUserId.Equals(userId))
                .OrderByDescending(item => item.CreatedDateTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<WatchlistItem> GetRandomUnwatchedItem(string userId)
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

        private IQueryable<WatchlistItem> GetUnwatchedItems(string userId)
        {
            return _dbContext.WatchlistItems
                .Where(item => item.CreatedByUserId == userId && !item.WatchedDateTime.HasValue);
        }
    }
}
