

using FluentValidation;
using SalesOrders.Domains.DTOs;

namespace SalesOrdersAPI.Validations
{
    public class UserRegRequestValidator : AbstractValidator<UserRegRequestDTO>
    {
        public UserRegRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                .WithMessage("Username is required");

            RuleFor(x => x.Password).NotEmpty()
              .WithMessage("Password is required");

            RuleFor(x => x.UserName).MaximumLength(100)
             .WithMessage("Invalid username");
        }
    }
}
