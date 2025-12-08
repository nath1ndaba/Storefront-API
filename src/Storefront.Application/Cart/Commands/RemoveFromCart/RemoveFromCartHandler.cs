using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Exceptions;
using Storefront.Application.Common.Interfaces;
using Storefront.Domain.Entities;

namespace Storefront.Application.Cart.Commands.RemoveFromCart;

public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveFromCartHandler> _logger;

    public RemoveFromCartHandler(IUnitOfWork unitOfWork, ILogger<RemoveFromCartHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing cart item {CartItemId}", request.CartItemId);

        var cart = await _unitOfWork.Carts.GetBySessionIdAsync(request.sessionId);

        if (cart == null)
        {
            _logger.LogWarning("Cart not found for session {SessionId}", request.sessionId);
            throw new NotFoundException(nameof(CartItem), request.CartItemId);
        }

        var cartItem = cart.Items?.FirstOrDefault(i => i.Id == request.CartItemId);
        if (cartItem == null)
        {
            _logger.LogWarning("Cart item {CartItemId} not found in session {SessionId}'s cart",
            request.CartItemId, request.sessionId);
            throw new NotFoundException(nameof(CartItem), request.CartItemId);
        }
        cart.Items?.Remove(cartItem);
        cart.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart item {CartItemId} removed successfully", request.CartItemId);

        return true;
    }
}
