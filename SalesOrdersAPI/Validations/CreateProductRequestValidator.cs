

using FluentValidation;
using SalesOrders.Domains.DTOs;

namespace SalesOrdersAPI.Validations
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Product name is required");

            RuleFor(x => x.Name).MaximumLength(100)
              .WithMessage("Invalid product name length");

            RuleFor(x => x.Price).Must((request, Price) =>
              Price > 0)
              .WithMessage("Invalid product price");
        }
    }
}
