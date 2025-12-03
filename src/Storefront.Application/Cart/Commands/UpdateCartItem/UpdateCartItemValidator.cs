using FluentValidation;

namespace Storefront.Application.Cart.Commands.UpdateCartItem;

public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemValidator()
    {
        RuleFor(x => x.CartItemId)
            .GreaterThan(0).WithMessage("Invalid cart item ID");

        RuleFor(x => x.UpdateDto.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100");
    }
}
