using MediatR;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(int Id, UpdateProductDto ProductDto) : IRequest<ProductDto>;
