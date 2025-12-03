using MediatR;

namespace Storefront.Application.Cart.Commands.ClearCart;

public record ClearCartCommand(string SessionId) : IRequest<bool>;
