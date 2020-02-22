using BookmarksApp.Infrastructure;
using BookmarksApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookmarksApp.Controllers {
    [Route("api/[controller]")]
    public class TagsController : ControllerBase {
        private readonly DatabaseRepository DB;
        public TagsController(DatabaseRepository db) =>
            DB = db;

        [HttpGet]
        public ActionResult<ICollection<string>> Get() {
            return DB.GetBookmarksData().SelectMany(bookmark => bookmark.Tags).Distinct().ToList();
        }

        [HttpGet("{name:alpha}")]
        public ActionResult<ICollection<Bookmark>> Get(string name) {
            return DB.GetBookmarksData().Where(bookmark => bookmark.Tags.Contains(name)).ToList();
        }
    }
}