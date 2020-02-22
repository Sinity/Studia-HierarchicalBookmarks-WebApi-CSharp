using FluentValidation;
using System.Collections.Generic;

namespace BookmarksApp.Messages.Requests {
    public class BookmarkRequest {
        public string URL { get; set; }
        public ICollection<string> Tags { get; set; }
    }

    public class BookmarkRequestValidator : AbstractValidator<BookmarkRequest> {
        public BookmarkRequestValidator() {
            RuleFor(x => x.URL).NotEmpty();
        }
    }
}