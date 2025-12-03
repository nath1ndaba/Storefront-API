using MediatR;

namespace Storefront.Application.Cart.Commands.RemoveFromCart;

public record RemoveFromCartCommand(int CartItemId) : IRequest<bool>;
