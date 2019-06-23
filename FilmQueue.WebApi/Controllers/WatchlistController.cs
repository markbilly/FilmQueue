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
    [ApiController]
    [Route("users/me/watchlist")]
    public class WatchlistController : ControllerBase
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IFilmReader _watchlistItemReader;

        public WatchlistController(
            ICurrentUserAccessor currentUserAccessor,
            IFilmReader watchlistItemReader)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchlistItemReader = watchlistItemReader;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QueryResponse<string>>), 200)]
        public async Task<IActionResult> GetWatchlist(int page = 1, int pageSize = 5)
        {
            var records = await _watchlistItemReader.GetItemsByUserId(_currentUserAccessor.CurrentUser.Id, pageSize, (page - 1) * pageSize);

            return Ok(QueryResponse<WatchlistItemResponse>.FromEnumerable(records, record => WatchlistItemResponse.FromRecord(record)));
        }
    }
}
