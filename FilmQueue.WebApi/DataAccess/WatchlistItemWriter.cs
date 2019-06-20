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
        Task<WatchlistItemRecord> Create(string userId, string title, int runtimeInMinutes);
        void SetWatchNextStartDateToNow(long id);
        void SetWatchNextEndDateToNow(long id);
        void SetWatchedDateToNow(long id);
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

        public async Task<WatchlistItemRecord> Create(string userId, string title, int runtimeInMinutes)
        {
            var created = await _dbContext.AddAsync(new WatchlistItemRecord
            {
                Title = title,
                RuntimeInMinutes = runtimeInMinutes,
                CreatedDateTime = _clock.UtcNow,
                CreatedByUserId = userId
            });

            return created.Entity;
        }

        public void SetWatchedDateToNow(long id)
        {
            var item = _dbContext.WatchlistItemRecords.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return;
            }

            item.WatchedDateTime = _clock.UtcNow;
        }

        public void SetWatchNextEndDateToNow(long id)
        {
            var item = _dbContext.WatchlistItemRecords.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return;
            }

            item.WatchNextEnd = _clock.UtcNow;
        }

        public void SetWatchNextStartDateToNow(long id)
        {
            var item = _dbContext.WatchlistItemRecords.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return;
            }

            item.WatchNextStart = _clock.UtcNow;
        }
    }
}
