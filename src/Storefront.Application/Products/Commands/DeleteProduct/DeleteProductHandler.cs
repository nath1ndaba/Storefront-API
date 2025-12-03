using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Exceptions;
using Storefront.Application.Common.Interfaces;

namespace Storefront.Application.Products.Commands.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IUnitOfWork unitOfWork, ILogger<DeleteProductHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with ID {ProductId}", request.Id);
        
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", request.Id);
            throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
        }
        
        // Soft delete
        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Product with ID {ProductId} deleted successfully", request.Id);
        
        return true;
    }
}
