using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

public static class ProveedorEndpoints
{
    public static void MapProveedorEndpoints(this WebApplication app)
    {
        app.MapGet("/api/proveedores", [Authorize] async (ProveedorService service) =>
            await service.GetAllAsync());

        app.MapGet("/api/proveedores/{id}", [Authorize] async (int id, ProveedorService service) =>
        {
            var proveedor = await service.GetByIdAsync(id);
            return proveedor is not null ? Results.Ok(proveedor) : Results.NotFound();
        });

        app.MapPost("/api/proveedores", [Authorize] async (
            [FromBody] Proveedor proveedor,
            IValidator<Proveedor> validator,
            ProveedorService service) =>
        {
            var result = await validator.ValidateAsync(proveedor);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var creado = await service.CreateAsync(proveedor);
            return Results.Created($"/api/proveedores/{creado.Id}", creado);
        });

        app.MapPut("/api/proveedores/{id}", [Authorize] async (
            int id,
            [FromBody] Proveedor proveedor,
            IValidator<Proveedor> validator,
            ProveedorService service) =>
        {
            var result = await validator.ValidateAsync(proveedor);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var actualizado = await service.UpdateAsync(id, proveedor);
            return actualizado ? Results.Ok(proveedor) : Results.NotFound();
        });

        app.MapDelete("/api/proveedores/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            ProveedorService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}