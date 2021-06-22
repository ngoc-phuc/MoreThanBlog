using Core.Model.Blog;
using FluentValidation;

namespace Core.Validator.Blog
{
    public class AddBlogModelValidator : AbstractValidator<AddBlogModel>
    {
        public AddBlogModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Desc).NotEmpty();
            RuleFor(x => x.CategoryIds).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.MainImageId).NotEmpty();
        }
    }
}