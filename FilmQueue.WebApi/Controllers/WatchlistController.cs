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
        public async Task<IActionResult> GetWatchlistItems(int page = 1, int pageSize = 5)
        {
            var items = await _watchlistReader.GetItemsByUserId(_currentUserAccessor.CurrentUser.Id, pageSize, (page - 1) * pageSize);

            return Ok(QueryResponse<WatchlistItem>.FromEnumerable(items, item => new WatchlistItem(item.Title, item.RuntimeInMinutes)));
        }

        [HttpGet("items/{id}")]
        [ProducesResponseType(typeof(WatchlistItem), 200)]
        public async Task<IActionResult> GetWatchlistItem([FromRoute] long id)
        {
            var item = await _watchlistReader.GetItemById(id);

            return Ok(new WatchlistItem(item.Title, item.RuntimeInMinutes));
        }

        [HttpGet("items/watchnext")]
        [ProducesResponseType(typeof(WatchlistItem), 200)]
        public async Task<IActionResult> GetWatchNext()
        {
            var item = await _watchlistReader.GetRandomUnwatchedItem();

            return Ok(new WatchlistItem(item.Title, item.RuntimeInMinutes));
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToWatchlist([FromBody] AddToWatchlistRequest addToWatchlistRequest)
        {
            var item = await _unitOfWork.Execute(async () =>
            {
                return await _watchlistItemWriter.Create(addToWatchlistRequest.Title, addToWatchlistRequest.RuntimeInMinutes);
            });

            return CreatedAtAction(nameof(GetWatchlistItem), new { id = item.Id }, new WatchlistItem(item.Title, item.RuntimeInMinutes));
        }
    }
}
