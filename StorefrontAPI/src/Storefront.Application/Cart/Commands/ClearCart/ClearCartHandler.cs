using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Exceptions;
using Storefront.Application.Common.Interfaces;

namespace Storefront.Application.Cart.Commands.ClearCart;

public class ClearCartHandler : IRequestHandler<ClearCartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClearCartHandler> _logger;

    public ClearCartHandler(IUnitOfWork unitOfWork, ILogger<ClearCartHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Clearing cart for session {SessionId}", request.SessionId);
        
        var cart = await _unitOfWork.Carts.GetBySessionIdAsync(request.SessionId);
        
        if (cart == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Cart), request.SessionId);
        }
        
        cart.Items.Clear();
        cart.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Cart cleared successfully for session {SessionId}", request.SessionId);
        
        return true;
    }
}
