using FilmQueue.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain
{
    public interface IWatchNextServiceFactory : IDependency
    {
        IWatchNextService GetForCurrentUser();
    }

    public class WatchNextServiceFactory : IWatchNextServiceFactory
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public WatchNextServiceFactory(ICurrentUserAccessor currentUserAccessor)
        {
            _currentUserAccessor = currentUserAccessor;
        }

        public IWatchNextService GetForCurrentUser()
        {
            return new WatchNextService(_currentUserAccessor.CurrentUser.Id);
        }
    }

    public interface IWatchNextService
    {
        Task<WatchlistItem> GetCurrentWatchNextItem();
        Task<WatchlistItem> SelectNewWatchNextItem();
        Task MarkCurrentWatchNextAsWatched();
    }

    public class WatchNextService : IWatchNextService
    {
        private readonly string _userId;

        public WatchNextService(string userId)
        {
            _userId = userId;
        }

        public Task<WatchlistItem> GetCurrentWatchNextItem()
        {
            // look for open watch next event and identify item

            // get the watchlist item

            throw new NotImplementedException();
        }

        public Task MarkCurrentWatchNextAsWatched()
        {
            // check that they do currently have a watch next item

            // add watchlist event to record that they watched the item

            // close watchlist event recording item as their watch next

            throw new NotImplementedException();
        }

        public Task<WatchlistItem> SelectNewWatchNextItem()
        {
            // check they do not currently have a watch next item

            // select a random item that has not been watched

            // add event to record item as their watch next

            throw new NotImplementedException();
        }
    }
}
