namespace Storefront.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICartRepository Carts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
