using System;
using FluentValidation;
using BookmarksApp.Models;
using System.Collections.Generic;

namespace BookmarksApp.Messages.Requests {
    public class TagRequest {
        public string Name { get; set; }
        public ICollection<Guid> Parents { get; set; }
    }

    public class TagRequestValidator : AbstractValidator<TagRequest> {
        public TagRequestValidator() {
            RuleFor(x => x.Name).Length(1, 1000).Matches("^[-\\w]+$");
            RuleFor(x => x.Parents).NotEmpty();
        }
    }
}