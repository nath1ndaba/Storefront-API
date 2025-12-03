using FluentValidation;

namespace Storefront.Application.Cart.Commands.AddToCart;

public class AddToCartValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartValidator()
    {
        RuleFor(x => x.CartDto.SessionId)
            .NotEmpty().WithMessage("Session ID is required");

        RuleFor(x => x.CartDto.ProductId)
            .GreaterThan(0).WithMessage("Invalid product ID");

        RuleFor(x => x.CartDto.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100");
    }
}
