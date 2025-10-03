using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

public static class ProductoEndpoints
{
    public static void MapProductoEndpoints(this WebApplication app)
    {
        // Obtener todos los productos
        app.MapGet("/api/productos", [Authorize] async (ProductoService service) =>
            await service.GetAllAsync());

        //  Obtener producto por ID
        app.MapGet("/api/productos/{id}", [Authorize] async (int id, ProductoService service) =>
        {
            var producto = await service.GetByIdAsync(id);
            return producto is not null ? Results.Ok(producto) : Results.NotFound();
        });

        //  Crear nuevo producto con validación
        app.MapPost("/api/productos", [Authorize] async (
            [FromBody] Producto producto,
            IValidator<Producto> validator,
            ProductoService service) =>
        {
            var result = await validator.ValidateAsync(producto);
            if (!result.IsValid)
                return Results.BadRequest(result.Errors);

            var creado = await service.CreateAsync(producto);
            return Results.Created($"/api/productos/{creado.Id}", creado);
        });

        //  Actualizar producto existente
        app.MapPut("/api/productos/{id}", [Authorize] async (
            int id,
            [FromBody] Producto producto,
            IValidator<Producto> validator,
            ProductoService service) =>
        {
            var result = await validator.ValidateAsync(producto);
            if (!result.IsValid)
                return Results.BadRequest(result.Errors);

            var actualizado = await service.UpdateAsync(id, producto);
            return actualizado ? Results.Ok(producto) : Results.NotFound();
        });

        //  Eliminar producto
        app.MapDelete("/api/productos/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            ProductoService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });

        // Obtener productos con bajo stock
        app.MapGet("/api/productos/bajo-stock/{umbral}", [Authorize] async (
            int umbral,
            ProductoService service) =>
        {
            var productos = await service.GetProductosBajoStockAsync(umbral);
            return Results.Ok(productos);
        });
    }
}