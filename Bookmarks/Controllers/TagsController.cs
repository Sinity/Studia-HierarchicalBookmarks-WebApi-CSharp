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
    public class TagsController : ControllerBase {
        private readonly DatabaseRepository DB;
        public TagsController(DatabaseRepository db) =>
            DB = db;

        [HttpGet]
        public ActionResult<ICollection<TagNodeResponse>> Get() {
            var tags = DB.GetTagsData();
            var top = tags.Where(tag => tag.Parents.Count == 0).Select(tag => new TagNodeResponse(tag.Id, tag.Name)).ToList();
            foreach (var tag in top)
                tag.LoadChildren(tags);
            return top;
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Tag> Get(Guid id) =>
            (ActionResult<Tag>)DB.GetTagsData().First(x => x.Id == id) ?? NotFound();

        [HttpPost]
        public async Task<ActionResult<String>> Post([FromBody] TagRequest request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var tag = new Tag (
                Guid.NewGuid(),
                request.Name,
                DB.GetTagsData().Where(x => request.Parents.Contains(x.Id)).ToArray()
            );

            await DB.Insert(tag);
            return Ok(tag.Id);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] TagRequest request) {
            var tag = DB.GetTagsData().FirstOrDefault(x => x.Id == id);
            if (tag == null)
                return NotFound();

            tag.Name = request.Name;
            tag.Parents = DB.GetTagsData().Where(x => request.Parents.Contains(x.Id)).ToArray();

            await DB.Update<Tag>(x => x.Id == id, tag);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id) {
            var deletedTag = DB.GetTagsData().FirstOrDefault(x => x.Id == id);
            if (deletedTag == null)
                return NotFound();

            var childrenTags = DB.GetTagsData().Any(x => x.Parents.Contains(deletedTag));
            var childrenBookmarks = DB.GetBookmarksData().Any(x => x.Tags.Contains(deletedTag));
            if (childrenTags || childrenBookmarks)
                return BadRequest();

            await DB.Delete<Tag>(x => x.Id == id);
            return Ok();
        }
    }
}