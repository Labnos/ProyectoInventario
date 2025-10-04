using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoInventario.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetAllAsync() =>
        await _db.Users.Where(u => u.Activo).ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await _db.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> CreateAsync(string username, string password, string role, string nombreCompleto, string correo, string telefono)
    {
        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            NombreCompleto = nombreCompleto,
            Correo = correo,
            Telefono = telefono,
            Activo = true
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(int id, User updatedUser)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;

        user.Username = updatedUser.Username;
        user.Role = updatedUser.Role;
        user.NombreCompleto = updatedUser.NombreCompleto;
        user.Correo = updatedUser.Correo;
        user.Telefono = updatedUser.Telefono;
        user.SucursalAsignada = updatedUser.SucursalAsignada;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return false;

        user.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }
}