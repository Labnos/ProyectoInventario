using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Services; // <-- LÍNEA AÑADIDA
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;

namespace ProyectoInventario.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/api/auth");

            // --- ENDPOINT DE LOGIN ---
            authGroup.MapPost("/login", async (
                [FromBody] UserLoginDto login,
                IValidator<UserLoginDto> validator,
                AppDbContext db,
                AuthService authService) => // <-- Ahora C# sabe qué es AuthService
            {
                var validationResult = await validator.ValidateAsync(login);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
                if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                {
                    return Results.Unauthorized();
                }

                var token = authService.GenerateJwtToken(user);
                return Results.Ok(new { Token = token });

            })
            .AllowAnonymous()
            .WithTags("Autenticación")
            .WithSummary("Inicia sesión para obtener un token de acceso JWT.")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest);


            // --- ENDPOINT PROTEGIDO PARA VERIFICAR EL TOKEN ---
            authGroup.MapGet("/verify", [Authorize] (ClaimsPrincipal user) =>
            {
                var username = user.Identity?.Name;
                var role = user.FindFirst(ClaimTypes.Role)?.Value;
                return Results.Ok(new { Username = username, Role = role });
            })
            .WithTags("Autenticación")
            .WithSummary("Verifica el token actual y devuelve la información del usuario.")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }

    // DTO para recibir las credenciales de login
    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}