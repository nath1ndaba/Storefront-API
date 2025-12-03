using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Cart.DTOs;
using Storefront.Application.Common.Exceptions;
using Storefront.Application.Common.Interfaces;
using Storefront.Domain.Entities;

namespace Storefront.Application.Cart.Commands.AddToCart;

public class AddToCartHandler : IRequestHandler<AddToCartCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddToCartHandler> _logger;

    public AddToCartHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddToCartHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding product {ProductId} to cart for session {SessionId}", 
            request.CartDto.ProductId, request.CartDto.SessionId);
        
        var product = await _unitOfWork.Products.GetByIdAsync(request.CartDto.ProductId);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.CartDto.ProductId);
        }
        
        var cart = await _unitOfWork.Carts.GetBySessionIdAsync(request.CartDto.SessionId);
        
        if (cart == null)
        {
            cart = new Domain.Entities.Cart
            {
                SessionId = request.CartDto.SessionId,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            // Reload to get the ID
            cart = await _unitOfWork.Carts.GetBySessionIdAsync(request.CartDto.SessionId);
        }
        
        // Check if product already in cart
        var existingItem = cart!.Items.FirstOrDefault(i => i.ProductId == request.CartDto.ProductId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += request.CartDto.Quantity;
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = request.CartDto.Quantity,
                UnitPrice = product.Price,
                CreatedAt = DateTime.UtcNow
            };
            cart.Items.Add(cartItem);
        }
        
        cart.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Reload cart with items
        cart = await _unitOfWork.Carts.GetCartWithItemsAsync(cart.Id);
        
        _logger.LogInformation("Product added to cart successfully");
        
        var cartDto = _mapper.Map<CartDto>(cart);
        cartDto.TotalAmount = cart!.TotalAmount;
        cartDto.TotalItems = cart.TotalItems;
        
        return cartDto;
    }
}
