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
        Task<WatchlistItem> GetRandomUnwatchedItem();
    }

    public class WatchlistReader : IWatchlistReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public WatchlistReader(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<WatchlistItem> GetRandomUnwatchedItem()
        {
            var unwatchedItems = _dbContext.WatchlistItems;

            var count = await unwatchedItems.CountAsync();
            var item = await unwatchedItems.Skip(new Random().Next(count)).FirstOrDefaultAsync();

            return item;
        }
    }
}
