using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain;
using FilmQueue.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmQueue.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/watchlist")]
    public class WatchlistController : ControllerBase
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IWatchlistItemWriter _watchlistItemWriter;
        private readonly IWatchlistReader _watchlistReader;
        private readonly IUnitOfWork _unitOfWork;

        public WatchlistController(
            ICurrentUserAccessor currentUserAccessor,
            IWatchlistItemWriter watchlistItemWriter,
            IWatchlistReader watchlistReader,
            IUnitOfWork unitOfWork)
        {
            _currentUserAccessor = currentUserAccessor;
            _watchlistItemWriter = watchlistItemWriter;
            _watchlistReader = watchlistReader;
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<QueryResponse<string>>), 200)]
        public async Task<IActionResult> GetWatchlistItems()
        {
            var items = await _watchlistReader.GetItemsByUserId(_currentUserAccessor.CurrentUser.Id);

            return Ok(new Watchlist
            {
                Items = items.Select(item => new Domain.WatchlistItem(item.Title, item.RuntimeInMinutes))
            });
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToWatchlist([FromBody] AddToWatchlistRequest addToWatchlistRequest)
        {


            return Ok();
        }
    }
}
