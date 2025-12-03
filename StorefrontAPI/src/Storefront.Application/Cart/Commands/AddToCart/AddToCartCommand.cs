using MediatR;
using Storefront.Application.Cart.DTOs;

namespace Storefront.Application.Cart.Commands.AddToCart;

public record AddToCartCommand(AddToCartDto CartDto) : IRequest<CartDto>;
