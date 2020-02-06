using BookmarksApp.Infrastructure;
using BookmarksApp.Messages.Requests;
using BookmarksApp.Messages.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookmarksApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarksController : ControllerBase {
        private readonly DatabaseRepository _repository;
        public BookmarksController(DatabaseRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<ICollection<BookmarkResponse>> Get([FromQuery] string tags, [FromQuery] string sort) {
            return _repository
                .GetBookmarksData()
                .Select(x => new BookmarkResponse(x.Id, x.URL, x.CreatedOn, x.Tags))
                .ToArray();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> Post([FromBody] CreateBookmarkRequest bookmarkRequest) {
            var bookmark = new BookmarksApp.Models.Bookmark (
                Guid.NewGuid(),
                bookmarkRequest.URL,
                DateTime.Now,
                _repository.GetTagsData().Where(x => bookmarkRequest.Tags.Contains(x.Id)).ToArray()
            );

            await _repository.Insert(bookmark);
            return Ok(bookmark.Id);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Put(Guid id, [FromBody] CreateBookmarkRequest bookmarkRequest) {
            var bookmark = _repository
                .GetBookmarksData()
                .FirstOrDefault(x => x.Id == id);

            if (bookmark == null)
                return NotFound();

            bookmark.URL = bookmarkRequest.URL;
            bookmark.Tags = _repository.GetTagsData().Where(x => bookmarkRequest.Tags.Contains(x.Id)).ToArray();

            await _repository.Update(x => x.Id == id, bookmark);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id) {
            var bookmark = _repository
                .GetBookmarksData()
                .FirstOrDefault(x => x.Id == id);

            if (bookmark == null)
                return NotFound();

            await _repository.Delete<BookmarksApp.Models.Bookmark>(x => x.Id == id);
            return Ok();
        }
    }
}