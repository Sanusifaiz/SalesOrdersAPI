

using FluentValidation;
using SalesOrders.Domains.DTOs;

namespace SalesOrdersAPI.Validations
{
    public class CreateSalesOrderRequestValidator : AbstractValidator<CreateSalesOrderDTO>
    {
        public CreateSalesOrderRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty()
                .WithMessage("Product Id is required");

            RuleFor(x => x.CustomerId).NotEmpty()
              .WithMessage("Customer Id is required");

            RuleFor(x => x.Quantity).NotEmpty()
             .WithMessage("Quantity is required");

            RuleFor(x => x.Quantity).Must((request, Quantity) =>
              Quantity > 0)
              .WithMessage("Invalid product quantity");
        }
    }
}
