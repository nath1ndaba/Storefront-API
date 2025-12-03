using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Interfaces;
using Storefront.Application.Products.DTOs;
using Storefront.Domain.Entities;

namespace Storefront.Application.Products.Commands.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateProductHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new product: {ProductName}", request.ProductDto.Name);
        
        var product = _mapper.Map<Product>(request.ProductDto);
        product.CreatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Product created with ID {ProductId}", product.Id);
        
        return _mapper.Map<ProductDto>(product);
    }
}
