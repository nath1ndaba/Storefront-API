using Storefront.Domain.Enums;

namespace Storefront.Application.Products.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public ProductCategory Category { get; set; }
    public int StockQuantity { get; set; }
    public string Sku { get; set; } = string.Empty;
}
