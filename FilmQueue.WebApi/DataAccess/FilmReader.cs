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

        public Task<FilmRecord> GetFilmById(long filmId)
        {
            return _dbContext.FilmRecords.FirstOrDefaultAsync(item => item.Id == filmId);
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
            return _dbContext.FilmRecords
                .Where(item => item.OwnedByUserId == userId && !item.WatchedDateTime.HasValue);
        }
    }
}
