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
                OwnedByUserId = userId
            });

            return created.Entity;
        }

        public void MarkFilmAsWatched(long filmId)
        {
            var filmRecord = _dbContext.FilmRecords.SingleOrDefault(x => x.Id == filmId);

            if (filmRecord == null)
            {
                return;
            }

            filmRecord.WatchedDateTime = _clock.UtcNow;
        }
    }
}
