using MediatR;
using Storefront.Application.Cart.DTOs;

namespace Storefront.Application.Cart.Queries.GetCart;

public record GetCartQuery(string SessionId) : IRequest<CartDto?>;
