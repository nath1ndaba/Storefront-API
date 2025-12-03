using MediatR;
using Storefront.Application.Common.DTOs;
using Storefront.Application.Products.Commands.CreateProduct;
using Storefront.Application.Products.Commands.DeleteProduct;
using Storefront.Application.Products.Commands.UpdateProduct;
using Storefront.Application.Products.DTOs;
using Storefront.Application.Products.Queries.GetAllProducts;
using Storefront.Application.Products.Queries.GetProductById;

namespace Storefront.API.Endpoints;

public static class ProductEndpoints
{
    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        group.MapGet("/", async (ISender sender) =>
        {
            var query = new GetAllProductsQuery();
            var result = await sender.Send(query);
            return Results.Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(result));
        })
        .WithName("GetAllProducts")
        .Produces<ApiResponse<IEnumerable<ProductDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, ISender sender) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.Send(query);
            
            if (result == null)
                return Results.NotFound(ApiResponse<ProductDto>.ErrorResponse($"Product with ID {id} not found"));
            
            return Results.Ok(ApiResponse<ProductDto>.SuccessResponse(result));
        })
        .WithName("GetProductById")
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateProductDto productDto, ISender sender) =>
        {
            var command = new CreateProductCommand(productDto);
            var result = await sender.Send(command);
            return Results.Created($"/api/products/{result.Id}", ApiResponse<ProductDto>.SuccessResponse(result, "Product created successfully"));
        })
        .WithName("CreateProduct")
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", async (int id, UpdateProductDto productDto, ISender sender) =>
        {
            var command = new UpdateProductCommand(id, productDto);
            var result = await sender.Send(command);
            return Results.Ok(ApiResponse<ProductDto>.SuccessResponse(result, "Product updated successfully"));
        })
        .WithName("UpdateProduct")
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<ProductDto>>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", async (int id, ISender sender) =>
        {
            var command = new DeleteProductCommand(id);
            var result = await sender.Send(command);
            return Results.Ok(ApiResponse<bool>.SuccessResponse(result, "Product deleted successfully"));
        })
        .WithName("DeleteProduct")
        .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound);

        return app;
    }
}
