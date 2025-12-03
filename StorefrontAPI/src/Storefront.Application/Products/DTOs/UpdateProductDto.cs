using Storefront.Domain.Enums;

namespace Storefront.Application.Products.DTOs;

public class UpdateProductDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public ProductCategory? Category { get; set; }
    public int? StockQuantity { get; set; }
}
