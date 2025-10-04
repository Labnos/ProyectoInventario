// Reemplaza todo el contenido de Endpoints/UserEndpoints.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation;

namespace ProyectoInventario.Endpoints;

// DTO para crear un usuario, recibe la contraseña en texto plano
public class UserCreateDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Vendedor";
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/usuarios").WithTags("Usuarios");

        // Obtener todos los usuarios
        group.MapGet("/", [Authorize(Policy = "AdminOnly")] async (UserService service) =>
            await service.GetAllAsync());

        // Obtener usuario por ID
        group.MapGet("/{id}", [Authorize(Policy = "AdminOnly")] async (int id, UserService service) =>
        {
            var user = await service.GetByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });

        // Crear nuevo usuario (CORREGIDO)
        group.MapPost("/", [Authorize(Policy = "AdminOnly")] async (
            [FromBody] UserCreateDto newUser,
            UserService service) =>
        {
            // El validador se puede aplicar aquí si es necesario, pero la lógica principal es llamar al servicio con los datos correctos
            var creado = await service.CreateAsync(
                newUser.Username, 
                newUser.Password, // Se pasa la contraseña sin encriptar
                newUser.Role,
                newUser.NombreCompleto,
                newUser.Correo,
                newUser.Telefono
            );

            if(creado is null) return Results.BadRequest("No se pudo crear el usuario.");

            return Results.Created($"/api/usuarios/{creado.Id}", creado);
        });

        // Actualizar usuario
        group.MapPut("/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            [FromBody] User user, // Para actualizar, se puede usar el modelo completo
            UserService service) =>
        {
            var actualizado = await service.UpdateAsync(id, user);
            return actualizado ? Results.Ok(user) : Results.NotFound();
        });

        // Cambiar contraseña
        group.MapPut("/{id}/password", [Authorize] async (
            int id,
            [FromBody] string nuevaPassword,
            UserService service) =>
        {
            var actualizado = await service.UpdatePasswordAsync(id, nuevaPassword);
            return actualizado ? Results.Ok("Contraseña actualizada") : Results.NotFound();
        });

        // Desactivar usuario
        group.MapDelete("/{id}", [Authorize(Policy = "AdminOnly")] async (
            int id,
            UserService service) =>
        {
            var eliminado = await service.DeactivateAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}