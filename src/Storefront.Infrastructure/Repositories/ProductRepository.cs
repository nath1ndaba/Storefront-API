using Microsoft.EntityFrameworkCore;
using Storefront.Application.Common.Interfaces;
using Storefront.Domain.Entities;
using Storefront.Infrastructure.Data;

namespace Storefront.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int category)
    {
        return await _dbSet
            .Where(p => (int)p.Category == category)
            .ToListAsync();
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Sku == sku);
    }
}
