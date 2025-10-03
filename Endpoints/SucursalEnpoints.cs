using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

public static class SucursalEndpoints
{
    public static void MapSucursalEndpoints(this WebApplication app)
    {
        app.MapGet("/api/sucursales", [Authorize] async (SucursalService service) =>
            await service.GetAllAsync());

        app.MapGet("/api/sucursales/{id}", [Authorize] async (int id, SucursalService service) =>
        {
            var sucursal = await service.GetByIdAsync(id);
            return sucursal is not null ? Results.Ok(sucursal) : Results.NotFound();
        });

        app.MapPost("/api/sucursales", [Authorize] async (
            [FromBody] Sucursal sucursal,
            IValidator<Sucursal> validator,
            SucursalService service) =>
        {
            var result = await validator.ValidateAsync(sucursal);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var creada = await service.CreateAsync(sucursal);
            return Results.Created($"/api/sucursales/{creada.Id}", creada);
        });

        app.MapPut("/api/sucursales/{id}", [Authorize] async (
            int id,
            [FromBody] Sucursal sucursal,
            IValidator<Sucursal> validator,
            SucursalService service) =>
        {
            var result = await validator.ValidateAsync(sucursal);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var actualizada = await service.UpdateAsync(id, sucursal);
            return actualizada ? Results.Ok(sucursal) : Results.NotFound();
        });

        app.MapDelete("/api/sucursales/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            SucursalService service) =>
        {
            var eliminada = await service.DeleteAsync(id);
            return eliminada ? Results.NoContent() : Results.NotFound();
        });
    }
}