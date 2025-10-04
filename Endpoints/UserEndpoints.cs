using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        //  Obtener todos los usuarios
        app.MapGet("/api/usuarios", [Authorize(Policy = "AdminOnly")] async (UserService service) =>
            await service.GetAllAsync());

        //  Obtener usuario por ID
        app.MapGet("/api/usuarios/{id}", [Authorize(Policy = "AdminOnly")] async (int id, UserService service) =>
        {
            var user = await service.GetByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });

        //  Crear nuevo usuario
        app.MapPost("/api/usuarios", [Authorize(Policy = "AdminOnly")] async (
            [FromBody] User user,
            IValidator<User> validator,
            UserService service) =>
        {
            var result = await validator.ValidateAsync(user);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var creado = await service.CreateAsync(user.Username, user.PasswordHash, user.Role);
            return Results.Created($"/api/usuarios/{creado.Id}", creado);
        });

        //  Actualizar usuario
        app.MapPut("/api/usuarios/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            [FromBody] User user,
            IValidator<User> validator,
            UserService service) =>
        {
            var result = await validator.ValidateAsync(user);
            if (!result.IsValid) return Results.BadRequest(result.Errors);

            var actualizado = await service.UpdateAsync(id, user);
            return actualizado ? Results.Ok(user) : Results.NotFound();
        });

        //  Cambiar contraseña
        app.MapPut("/api/usuarios/{id}/password", [Authorize] async (
            int id,
            [FromBody] string nuevaPassword,
            UserService service) =>
        {
            var actualizado = await service.UpdatePasswordAsync(id, nuevaPassword);
            return actualizado ? Results.Ok("Contraseña actualizada") : Results.NotFound();
        });

        //  Desactivar usuario
        app.MapDelete("/api/usuarios/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            UserService service) =>
        {
            var eliminado = await service.DeactivateAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}