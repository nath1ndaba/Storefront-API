namespace Storefront.Application.Cart.DTOs;

public class AddToCartDto
{
    public string SessionId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
