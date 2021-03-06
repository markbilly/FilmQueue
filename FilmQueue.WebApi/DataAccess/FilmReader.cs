﻿using FilmQueue.WebApi.DataAccess.Models;
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
        Task<FilmRecord> GetRandomUnwatchedFilm(string userId, long? excludeFilmId = null);
        Task<int> GetUnwatchedFilmCount(string userId);
        Task<int> GetWatchedFilmCount(string userId);
        Task<IEnumerable<FilmRecord>> GetWatched(string userId, int take, int skip);
    }

    public class FilmReader : IFilmReader
    {
        private readonly FilmQueueDbContext _dbContext;

        public FilmReader(FilmQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<FilmRecord> GetFilmById(long filmId)
        {
            return _dbContext.FilmRecords.FirstOrDefaultAsync(item => item.Id == filmId);
        }

        public async Task<FilmRecord> GetRandomUnwatchedFilm(string userId, long? excludeFilmId = null)
        {
            var unwatchedItems = GetUnwatchedItems(userId);

            if (excludeFilmId.HasValue)
            {
                unwatchedItems = unwatchedItems.Where(x => x.Id != excludeFilmId);
            }

            var count = await unwatchedItems.CountAsync();
            var randomItem = await unwatchedItems.Skip(new Random().Next(count)).FirstOrDefaultAsync();

            return randomItem;
        }

        public async Task<IEnumerable<FilmRecord>> GetWatched(string userId, int take, int skip)
        {
            return await _dbContext.FilmRecords
                .Where(x => x.OwnedByUserId == userId && x.WatchedDateTime.HasValue)
                .OrderByDescending(x => x.WatchedDateTime.Value)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
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

        public Task<int> GetWatchedFilmCount(string userId)
        {
            return _dbContext.FilmRecords
                .Where(item => item.OwnedByUserId == userId && item.WatchedDateTime.HasValue)
                .CountAsync();
        }
    }
}
