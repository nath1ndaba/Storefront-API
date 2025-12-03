using MediatR;
using Storefront.Application.Products.DTOs;

namespace Storefront.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
