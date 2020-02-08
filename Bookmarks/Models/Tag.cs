using System;
using System.Collections.Generic;

namespace BookmarksApp.Models {
    public class Tag {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Tag> Parents { get; set; }

        public Tag(Guid id, string name, ICollection<Tag> parents = null) {
            Id = id;
            Name = name;
            Parents = parents ?? new List<Tag>();
        }
    }
}