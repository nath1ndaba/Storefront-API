using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Storefront.Application.Common.Exceptions;
using Storefront.Application.Common.Interfaces;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Commands.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateProductHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product with ID {ProductId}", request.Id);
        
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", request.Id);
            throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
        }
        
        _mapper.Map(request.ProductDto, product);
        product.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Product with ID {ProductId} updated successfully", request.Id);
        
        return _mapper.Map<ProductDto>(product);
    }
}
