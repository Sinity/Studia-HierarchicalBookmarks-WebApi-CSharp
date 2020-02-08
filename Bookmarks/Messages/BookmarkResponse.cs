using BookmarksApp.Models;
using System;
using System.Collections.Generic;

namespace BookmarksApp.Messages.Responses {
    public class BookmarkResponse {
        public Guid Id { get; }
        public string URL { get; }
        public DateTime CreatedOn { get; }
        public ICollection<Tag> Tags { get; set; }

        public BookmarkResponse(Guid id, string url, DateTime createdOn, ICollection<Tag> tags = null) {
            Id = id;
            URL = url;
            CreatedOn = createdOn;
            Tags = tags ?? new List<Tag>();
        }
    }
}