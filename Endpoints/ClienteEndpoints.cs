using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints(this WebApplication app)
    {
        app.MapGet("/api/clientes", [Authorize] async (ClienteService service) =>
            await service.GetAllAsync());

        app.MapGet("/api/clientes/{id}", [Authorize] async (int id, ClienteService service) =>
        {
            var cliente = await service.GetByIdAsync(id);
            return cliente is not null ? Results.Ok(cliente) : Results.NotFound();
        });

        app.MapPost("/api/clientes", [Authorize] async (
            [FromBody] Cliente cliente,
            IValidator<Cliente> validator,
            ClienteService service) =>
        {
            var result = await validator.ValidateAsync(cliente);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var creado = await service.CreateAsync(cliente);
            return Results.Created($"/api/clientes/{creado.Id}", creado);
        });

        app.MapPut("/api/clientes/{id}", [Authorize] async (
            int id,
            [FromBody] Cliente cliente,
            IValidator<Cliente> validator,
            ClienteService service) =>
        {
            var result = await validator.ValidateAsync(cliente);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var actualizado = await service.UpdateAsync(id, cliente);
            return actualizado ? Results.Ok(cliente) : Results.NotFound();
        });

        app.MapDelete("/api/clientes/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            ClienteService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}