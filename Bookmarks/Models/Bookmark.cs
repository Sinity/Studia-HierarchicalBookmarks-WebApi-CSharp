using System;
using System.Collections.Generic;

namespace BookmarksApp.Models {
    public class Bookmark {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<string> Tags { get; set; }

        public Bookmark(Guid id, string URL, DateTime createdOn, ICollection<string> tags = null) {
            Id = id;
            this.URL = URL;
            CreatedOn = createdOn;
            Tags = tags ?? new List<string>();
        }
    }
}