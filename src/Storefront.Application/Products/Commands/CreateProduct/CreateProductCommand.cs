using MediatR;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto ProductDto) : IRequest<ProductDto>;
