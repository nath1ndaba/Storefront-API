using Storefront.Domain.Common;

namespace Storefront.Domain.Entities;

public class Cart : BaseEntity
{
    public string SessionId { get; set; } = string.Empty;
    public ICollection<CartItem> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(item => item.Subtotal);
    public int TotalItems => Items.Sum(item => item.Quantity);
}
