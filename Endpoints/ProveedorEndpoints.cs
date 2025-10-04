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
        var group = app.MapGroup("/api/proveedores").WithTags("Proveedores");

        // --- ENDPOINT MODIFICADO para paginación ---
        group.MapGet("/", [Authorize] async (
            [FromQuery] int? pageNumber, 
            [FromQuery] int? pageSize, 
            ProveedorService service) =>
        {
            int page = pageNumber.HasValue && pageNumber.Value > 0 ? pageNumber.Value : 1;
            int size = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 10;
            return await service.GetAllAsync(page, size);
        });

        group.MapGet("/{id}", [Authorize] async (int id, ProveedorService service) =>
        {
            var proveedor = await service.GetByIdAsync(id);
            return proveedor is not null ? Results.Ok(proveedor) : Results.NotFound();
        });

        group.MapPost("/", [Authorize] async (Proveedor proveedor, ProveedorService service, IValidator<Proveedor> validator) =>
        {
            var validationResult = await validator.ValidateAsync(proveedor);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
            
            var created = await service.CreateAsync(proveedor);
            return Results.Created($"/api/proveedores/{created.Id}", created);
        });

        group.MapPut("/{id}", [Authorize] async (int id, Proveedor proveedor, ProveedorService service, IValidator<Proveedor> validator) =>
        {
            var validationResult = await validator.ValidateAsync(proveedor);
            if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
            
            var updated = await service.UpdateAsync(id, proveedor);
            return updated ? Results.Ok(proveedor) : Results.NotFound();
        });

        group.MapDelete("/{id}", [Authorize] async (int id, ProveedorService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}