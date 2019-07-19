using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Domain.Events;
using FilmQueue.WebApi.Domain.Models;
using FilmQueue.WebApi.Models.Requests;
using FilmQueue.WebApi.Models.Responses;
using FilmQueue.WebApi.Infrastructure;
using FilmQueue.WebApi.Infrastructure.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [Route("users/me/watchlist")]
    public class WatchlistController : ApiController
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWatchlistReader _watchlistReader;
        private readonly IFilmReader _filmReader;

        public WatchlistController(
            ICurrentUserAccessor currentUserAccessor,
            IWatchlistReader watchlistReader,
            IFilmReader filmReader)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchlistReader = watchlistReader;
            _filmReader = filmReader;
        }
        
        /// <summary>
        /// Get your watchlist
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<FilmResponse>), 200)]
        public async Task<IActionResult> GetWatchlist(int page = 1, int pageSize = 5)
        {
            var count = await _filmReader.GetUnwatchedFilmCount(_currentUserAccessor.CurrentUser.Id);
            var records = await _watchlistReader.GetWatchlist(_currentUserAccessor.CurrentUser.Id, pageSize, (page - 1) * pageSize);

            return Ok(PagedResponse<FilmResponse>.FromEnumerable(records, record => FilmResponse.FromRecord(record), page, pageSize, count));
        }
    }
}
