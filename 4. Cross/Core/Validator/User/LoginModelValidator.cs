using Core.Model.User;
using FluentValidation;

namespace Core.Validator.User
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}