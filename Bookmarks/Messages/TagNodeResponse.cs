using BookmarksApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookmarksApp.Messages.Responses {
    public class TagNodeResponse {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<TagNodeResponse> Children { get; set; }

        public TagNodeResponse(Guid id, string name) {
            Id = id;
            Name = name;
        }

        public void LoadChildren(IQueryable<Tag> tags) {
            Children = tags.Where(tag => tag.Parents.Any(parent => parent.Id == Id)).Select(tag => new TagNodeResponse(tag.Id, tag.Name)).ToArray();
            foreach (var child in Children)
                child.LoadChildren(tags);
        }
    }
}