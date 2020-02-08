using BookmarksApp.Infrastructure;
using BookmarksApp.Messages.Requests;
using BookmarksApp.Messages.Responses;
using BookmarksApp.Models;
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
        private readonly DatabaseRepository DB;
        public BookmarksController(DatabaseRepository db) =>
            DB = db;

        [HttpGet]
        public ActionResult<ICollection<Bookmark>> Get([FromQuery] string tags, [FromQuery] string sort) {
            return DB.GetBookmarksData().ToArray();
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Bookmark> Get(Guid id) =>
            (ActionResult<Bookmark>)DB.GetBookmarksData().First(x => x.Id == id) ?? NotFound();

        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] BookmarkRequest bookmarkRequest) {
            var bookmark = new BookmarksApp.Models.Bookmark (
                Guid.NewGuid(),
                bookmarkRequest.URL,
                DateTime.Now,
                DB.GetTagsData().Where(x => bookmarkRequest.Tags.Contains(x.Id)).ToArray()
            );

            await DB.Insert(bookmark);
            return Ok(bookmark.Id);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] BookmarkRequest bookmarkRequest) {
            var bookmark = DB.GetBookmarksData().FirstOrDefault(x => x.Id == id);
            if (bookmark == null)
                return NotFound();

            bookmark.URL = bookmarkRequest.URL;
            bookmark.Tags = DB.GetTagsData().Where(x => bookmarkRequest.Tags.Contains(x.Id)).ToArray();

            await DB.Update(x => x.Id == id, bookmark);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id) {
            var bookmark = DB.GetBookmarksData().FirstOrDefault(x => x.Id == id);

            if (bookmark == null)
                return NotFound();

            await DB.Delete<BookmarksApp.Models.Bookmark>(x => x.Id == id);
            return Ok();
        }
    }
}