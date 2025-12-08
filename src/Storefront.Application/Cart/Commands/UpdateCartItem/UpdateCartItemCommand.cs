using MediatR;
using Storefront.Application.Cart.DTOs;

namespace Storefront.Application.Cart.Commands.UpdateCartItem;

public record UpdateCartItemCommand(int CartItemId, string sessionId, UpdateCartItemDto UpdateDto) : IRequest<CartItemDto>;
