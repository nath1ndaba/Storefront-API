using MediatR;
using Microsoft.AspNetCore.Mvc;
using Storefront.Application.Cart.Commands.AddToCart;
using Storefront.Application.Cart.Commands.ClearCart;
using Storefront.Application.Cart.Commands.RemoveFromCart;
using Storefront.Application.Cart.Commands.UpdateCartItem;
using Storefront.Application.Cart.DTOs;
using Storefront.Application.Cart.Queries.GetCart;
using Storefront.Application.Common.DTOs;

namespace Storefront.API.Endpoints;

public static class CartEndpoints
{
    public static WebApplication MapCartEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cart")
            .WithTags("Cart");

        group.MapGet("/", async (string sessionId, ISender sender) =>
        {
            var query = new GetCartQuery(sessionId);
            var result = await sender.Send(query);

            if (result == null)
                return Results.NotFound(ApiResponse<CartDto>.ErrorResponse("Cart not found"));

            return Results.Ok(ApiResponse<CartDto>.SuccessResponse(result));
        })
        .WithName("GetCart")
        .Produces<ApiResponse<CartDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<CartDto>>(StatusCodes.Status404NotFound);

        group.MapPost("/", async (AddToCartDto cartDto, ISender sender) =>
        {
            var command = new AddToCartCommand(cartDto);
            var result = await sender.Send(command);
            return Results.Created("/api/cart", ApiResponse<CartDto>.SuccessResponse(result, "Item added to cart successfully"));
        })
        .WithName("AddToCart")
        .Produces<ApiResponse<CartDto>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<CartDto>>(StatusCodes.Status400BadRequest);

        group.MapPatch("/{itemId:int}", async (int itemId, [FromQuery] string sessionId, [FromBody] UpdateCartItemDto updateDto, ISender sender) =>
        {
            var command = new UpdateCartItemCommand(itemId, sessionId, updateDto);
            var result = await sender.Send(command);
            return Results.Ok(ApiResponse<CartItemDto>.SuccessResponse(result, "Cart item updated successfully"));
        })
        .WithName("UpdateCartItem")
        .Produces<ApiResponse<CartItemDto>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<CartItemDto>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<CartItemDto>>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{itemId:int}", async (int itemId, ISender sender) =>
        {
            var command = new RemoveFromCartCommand(itemId);
            var result = await sender.Send(command);
            return Results.Ok(ApiResponse<bool>.SuccessResponse(result, "Item removed from cart successfully"));
        })
        .WithName("RemoveFromCart")
        .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound);

        group.MapDelete("/", async (string sessionId, ISender sender) =>
        {
            var command = new ClearCartCommand(sessionId);
            var result = await sender.Send(command);
            return Results.Ok(ApiResponse<bool>.SuccessResponse(result, "Cart cleared successfully"));
        })
        .WithName("ClearCart")
        .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound);

        return app;
    }
}
