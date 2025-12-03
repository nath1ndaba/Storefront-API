using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Interfaces;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Queries.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsHandler> _logger;

    public GetAllProductsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllProductsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all products");
        
        var products = await _unitOfWork.Products.GetAllAsync();
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        
        _logger.LogInformation("Retrieved {Count} products", productDtos.Count());
        
        return productDtos;
    }
}
