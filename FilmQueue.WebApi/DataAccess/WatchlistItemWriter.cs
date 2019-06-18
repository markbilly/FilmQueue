using FilmQueue.WebApi.DataAccess.Models;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.DataAccess
{
    public interface IWatchlistItemWriter : IDependency
    {
        Task Create(string title, int runtimeInMinutes);
    }

    public class WatchlistItemWriter : IWatchlistItemWriter
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly FilmQueueDbContext _dbContext;
        private readonly IClock _clock;

        public WatchlistItemWriter(
            ICurrentUserAccessor currentUserAccessor,
            FilmQueueDbContext dbContext,
            IClock clock)
        {
            _currentUserAccessor = currentUserAccessor;
            _dbContext = dbContext;
            _clock = clock;
        }

        public Task Create(string title, int runtimeInMinutes)
        {
            return _dbContext.AddAsync(new DataAccess.Models.WatchlistItem
            {
                Title = title,
                RuntimeInMinutes = runtimeInMinutes,
                CreatedDateTime = _clock.UtcNow,
                CreatedByUserId = _currentUserAccessor.CurrentUser.Id
            });
        }
    }
}
