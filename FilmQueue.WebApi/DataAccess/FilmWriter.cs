using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface IFilmWriter : IDependency
    {
        Task<FilmRecord> Create(string userId, string title, int runtimeInMinutes);
        void MarkFilmAsWatched(long filmId);

        // Move to WatchNextWriter
        void SetWatchNextStartDateToNow(long id);
        void SetWatchNextEndDateToNow(long id);
    }

    public class FilmWriter : IFilmWriter
    {
        private readonly FilmQueueDbContext _dbContext;
        private readonly IClock _clock;

        public FilmWriter(
            FilmQueueDbContext dbContext,
            IClock clock)
        {
            _dbContext = dbContext;
            _clock = clock;
        }

        public async Task<FilmRecord> Create(string userId, string title, int runtimeInMinutes)
        {
            var created = await _dbContext.AddAsync(new FilmRecord
            {
                Title = title,
                RuntimeInMinutes = runtimeInMinutes,
                CreatedDateTime = _clock.UtcNow,
                CreatedByUserId = userId
            });

            return created.Entity;
        }

        public void MarkFilmAsWatched(long filmId)
        {
            var item = _dbContext.WatchlistItemRecords.SingleOrDefault(x => x.Id == filmId);

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
