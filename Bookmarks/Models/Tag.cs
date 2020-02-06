using System;
using System.Collections.Generic;

namespace BookmarksApp.Models {
    public class Tag {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Tag Parent { get; set; }

        public Tag(Guid id, string name, Tag parent = default) {
            Id = id;
            Name = name;
            Parent = parent;
        }
    }
}