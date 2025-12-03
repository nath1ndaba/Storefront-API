using MediatR;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
