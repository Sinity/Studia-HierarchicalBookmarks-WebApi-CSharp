using System;
using FluentValidation;
using BookmarksApp.Models;
using System.Collections.Generic;

namespace BookmarksApp.Messages.Requests {
    public class BookmarkRequest {
        public string URL { get; set; }
        public ICollection<Guid> Tags { get; set; }
    }

    public class BookmarkRequestValidator : AbstractValidator<BookmarkRequest> {
        public BookmarkRequestValidator() {
            RuleFor(x => x.URL).NotEmpty();
            RuleFor(x => x.Tags).NotEmpty();
        }
    }
}