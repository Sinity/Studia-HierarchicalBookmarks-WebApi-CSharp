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
        private readonly DatabaseRepository _repository;
        public TagsController(DatabaseRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<ICollection<TagNodeResponse>> Get() {
            var tags = _repository.GetTagsData();
            var top = tags.Where(tag => tag.Parent == default).Select(tag => new TagNodeResponse(tag.Id, tag.Name)).ToArray();
            foreach (var tag in top)
                tag.LoadChildren(tags);
            return top;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<String>> Post([FromBody] CreateTagRequest request) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var tag = new Tag (
                Guid.NewGuid(),
                request.Name,
                _repository.GetTagsData().FirstOrDefault(x => x.Id == request.Parent)
            );

            await _repository.Insert(tag);
            return Ok(tag.Id);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Put(Guid id, [FromBody] CreateTagRequest request) {
            var tag = _repository
                .GetTagsData()
                .FirstOrDefault(x => x.Id == id);

            if (tag == null)
                return NotFound();

            tag.Name = request.Name;
            tag.Parent = _repository.GetTagsData().FirstOrDefault(x => x.Id == request.Parent);

            await _repository.Update<Tag>(x => x.Id == id, tag);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid id) {
            var deletedTag = _repository
                .GetTagsData()
                .FirstOrDefault(x => x.Id == id);

            if (deletedTag == null)
                return NotFound();

            var childrenTags =_repository.GetTagsData().Where(x => x.Parent == deletedTag);
            foreach (var child in childrenTags) {
                child.Parent = deletedTag.Parent;
                await _repository.Update<Tag>(x => x.Id == child.Id, child);
            }

            var childrenBookmarks = _repository.GetBookmarksData().Where(x => x.Tags.Contains(deletedTag));
            foreach (var child in childrenBookmarks) {
                child.Tags.Remove(deletedTag);
                child.Tags.Add(deletedTag.Parent);
                await _repository.Update<Bookmark>(x => x.Id == child.Id, child);
            }

            await _repository.Delete<Tag>(x => x.Id == id);
            return Ok();
        }
    }
}