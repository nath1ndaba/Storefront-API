using FluentValidation;

namespace Storefront.Application.Products.Commands.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid product ID");

        RuleFor(x => x.ProductDto.Name)
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.ProductDto.Name));

        RuleFor(x => x.ProductDto.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .When(x => x.ProductDto.Price.HasValue);

        RuleFor(x => x.ProductDto.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative")
            .When(x => x.ProductDto.StockQuantity.HasValue);
    }
}
