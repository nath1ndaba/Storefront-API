using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Cart.DTOs;
using Storefront.Application.Common.Exceptions;
using Storefront.Application.Common.Interfaces;
using Storefront.Domain.Entities;

namespace Storefront.Application.Cart.Commands.UpdateCartItem;

public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, CartItemDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCartItemHandler> _logger;

    public UpdateCartItemHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCartItemHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartItemDto> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating cart item {CartItemId}", request.CartItemId);

        var cart = await _unitOfWork.Carts.GetBySessionIdAsync(request.sessionId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found for session {SessionId}", request.sessionId);
            throw new NotFoundException("Cart", request.sessionId);
        }

        // Find the cart item within THIS session's cart
        var cartItem = cart.Items?.FirstOrDefault(i => i.Id == request.CartItemId);

        if (cartItem == null)
        {
            _logger.LogWarning("Cart item {CartItemId} not found in session {SessionId}'s cart",
            request.CartItemId, request.sessionId);
            throw new NotFoundException(nameof(CartItem), request.CartItemId);
        }

        cartItem.Quantity = request.UpdateDto.Quantity;
        cartItem.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cart item {CartItemId} updated successfully", request.CartItemId);

        return _mapper.Map<CartItemDto>(cartItem);
    }
}
