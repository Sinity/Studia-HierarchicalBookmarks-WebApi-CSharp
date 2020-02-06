using System;
using FluentValidation;
using BookmarksApp.Models;
using System.Collections.Generic;

namespace BookmarksApp.Messages.Requests {
    public class CreateTagRequest {
        public string Name { get; set; }
        public Guid Parent { get; set; }
    }

    public class TagValidator : AbstractValidator<Tag> {
        public TagValidator() {
            RuleFor(x => x.Name).NotEmpty().Length(1, 50).Matches("^[-\\w]+$");
        }
    }

    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest> {
        public CreateTagRequestValidator() {
            RuleFor(x => x.Name).NotEmpty().Length(1, 50).Matches("^[-\\w]+$");
        }
    }
}