using Microsoft.AspNetCore.Authorization;
using ProyectoInventario.Models;
using ProyectoInventario.Services;

namespace ProyectoInventario.Endpoints;

public static class PromocionEndpoints
{
    public static void MapPromocionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/promociones").WithTags("Promociones");

        group.MapGet("/", [Authorize] async (PromocionService service) => await service.GetAllAsync());

        group.MapGet("/{id}", [Authorize] async (int id, PromocionService service) =>
        {
            var promocion = await service.GetByIdAsync(id);
            return promocion is not null ? Results.Ok(promocion) : Results.NotFound();
        });

        group.MapPost("/", [Authorize] async (Promocion promocion, PromocionService service) =>
        {
            var created = await service.CreateAsync(promocion);
            return Results.Created($"/api/promociones/{created.Id}", created);
        });

        group.MapPut("/{id}", [Authorize] async (int id, Promocion promocion, PromocionService service) =>
        {
            var updated = await service.UpdateAsync(id, promocion);
            return updated ? Results.Ok(promocion) : Results.NotFound();
        });

        group.MapDelete("/{id}", [Authorize] async (int id, PromocionService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}