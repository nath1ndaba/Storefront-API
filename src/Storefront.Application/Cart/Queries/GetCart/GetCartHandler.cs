using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Cart.DTOs;
using Storefront.Application.Common.Interfaces;

namespace Storefront.Application.Cart.Queries.GetCart;

public class GetCartHandler : IRequestHandler<GetCartQuery, CartDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCartHandler> _logger;

    public GetCartHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCartHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartDto?> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching cart for session {SessionId}", request.SessionId);
        
        var cart = await _unitOfWork.Carts.GetBySessionIdAsync(request.SessionId);
        
        if (cart == null)
        {
            _logger.LogWarning("Cart not found for session {SessionId}", request.SessionId);
            return null;
        }
        
        var cartDto = _mapper.Map<CartDto>(cart);
        cartDto.TotalAmount = cart.TotalAmount;
        cartDto.TotalItems = cart.TotalItems;
        
        return cartDto;
    }
}
