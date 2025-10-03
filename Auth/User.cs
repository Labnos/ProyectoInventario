using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Auth;

public class Users
{
    private readonly AppDbContext _db;

    public Users(AppDbContext db)
    {
        _db = db;
    }

    // Buscar usuario por nombre
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    // Crear nuevo usuario con contraseña cifrada
    public async Task<User> CreateAsync(string username, string password, string role = "Encargado")
    {
        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            Activo = true
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    // Verificar credenciales
    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user is null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    // Cambiar contraseña
    public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _db.SaveChangesAsync();
        return true;
    }

    // Desactivar usuario
    public async Task<bool> DeactivateAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return false;

        user.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }
}