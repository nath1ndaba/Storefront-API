using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Interfaces;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetProductByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching product with ID {ProductId}", request.Id);
        
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", request.Id);
            return null;
        }
        
        return _mapper.Map<ProductDto>(product);
    }
}
