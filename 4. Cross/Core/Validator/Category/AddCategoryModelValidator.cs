using Core.Model.Category;
using FluentValidation;

namespace Core.Validator.Category
{
    public class AddCategoryModelValidator : AbstractValidator<AddCategoryModel>
    {
        public AddCategoryModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Desc).NotEmpty();
        }
    }
}