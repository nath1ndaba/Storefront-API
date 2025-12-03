using Storefront.Domain.Entities;

namespace Storefront.Application.Common.Interfaces;

public interface ICartRepository : IGenericRepository<Domain.Entities.Cart>
{
    Task<Domain.Entities.Cart?> GetBySessionIdAsync(string sessionId);
    Task<Domain.Entities.Cart?> GetCartWithItemsAsync(int cartId);
}
