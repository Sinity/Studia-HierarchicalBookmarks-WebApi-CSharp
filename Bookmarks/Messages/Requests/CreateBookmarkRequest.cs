using System;
using FluentValidation;
using BookmarksApp.Models;
using System.Collections.Generic;

namespace BookmarksApp.Messages.Requests {
    public class CreateBookmarkRequest {
        public string URL { get; set; }
        public ICollection<Guid> Tags { get; set; }
    }

    public class CreateBookmarkRequestValidator : AbstractValidator<CreateBookmarkRequest> {
        public CreateBookmarkRequestValidator() {
            RuleFor(x => x.URL).NotEmpty();
        }
    }
}