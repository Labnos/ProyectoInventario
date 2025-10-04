using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

public static class ProductoEndpoints
{
    public static void MapProductoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/productos").WithTags("Productos");

        // --- ENDPOINT MODIFICADO para paginación ---
        group.MapGet("/", [Authorize] async (
            [FromQuery] int? pageNumber, 
            [FromQuery] int? pageSize, 
            ProductoService service) =>
        {
            int page = pageNumber.HasValue && pageNumber.Value > 0 ? pageNumber.Value : 1;
            int size = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 10;
            return await service.GetAllAsync(page, size);
        });

        group.MapGet("/{id}", [Authorize] async (int id, ProductoService service) =>
        {
            var producto = await service.GetByIdAsync(id);
            return producto is not null ? Results.Ok(producto) : Results.NotFound();
        });

        group.MapPost("/", [Authorize] async (Producto producto, ProductoService service, IValidator<Producto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(producto);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
            
            var createdProducto = await service.CreateAsync(producto);
            return Results.Created($"/api/productos/{createdProducto.Id}", createdProducto);
        });

        group.MapPut("/{id}", [Authorize] async (int id, Producto producto, ProductoService service, IValidator<Producto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(producto);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

            var updated = await service.UpdateAsync(id, producto);
            return updated ? Results.Ok(producto) : Results.NotFound();
        });

        group.MapDelete("/{id}", [Authorize] async (int id, ProductoService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}