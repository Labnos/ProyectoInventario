using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;
using ProyectoInventario.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoInventario.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // DTO para login
        app.MapPost("/api/login", async (
            [FromBody] UserLoginDto login,
            AppDbContext db,
            AuthService auth) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                return Results.Unauthorized();

            var token = auth.GenerateToken(user);
            return Results.Ok(new { token });
        });

        // Endpoint protegido para verificar autenticación
        app.MapGet("/api/auth/verify", [Authorize] (ClaimsPrincipal user) =>
        {
            var username = user.Identity?.Name;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            return Results.Ok(new { username, role });
        });
    }
}

// DTO para login
public class UserLoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}