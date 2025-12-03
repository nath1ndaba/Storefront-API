using Microsoft.EntityFrameworkCore;
using Storefront.Application.Common.Interfaces;
using Storefront.Domain.Entities;
using Storefront.Infrastructure.Data;

namespace Storefront.Infrastructure.Repositories;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Cart?> GetBySessionIdAsync(string sessionId)
    {
        return await _dbSet
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);
    }

    public async Task<Cart?> GetCartWithItemsAsync(int cartId)
    {
        return await _dbSet
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }
}
