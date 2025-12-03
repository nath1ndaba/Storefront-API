using Storefront.Domain.Entities;

namespace Storefront.Application.Common.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(int category);
    Task<Product?> GetBySkuAsync(string sku);
}
