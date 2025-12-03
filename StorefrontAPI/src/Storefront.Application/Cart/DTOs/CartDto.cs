namespace Storefront.Application.Cart.DTOs;

public class CartDto
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}
