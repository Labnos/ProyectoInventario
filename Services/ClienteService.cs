using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class ClienteService
{
    private readonly AppDbContext _db;

    public ClienteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Cliente>> GetAllAsync() =>
        await _db.Clientes.Include(c => c.Ventas).ToListAsync();

    public async Task<Cliente?> GetByIdAsync(int id) =>
        await _db.Clientes.Include(c => c.Ventas).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Cliente> CreateAsync(Cliente cliente)
    {
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> UpdateAsync(int id, Cliente updated)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente is null) return false;

        cliente.Nombre = updated.Nombre;
        cliente.Telefono = updated.Telefono;
        cliente.Direccion = updated.Direccion;
        cliente.Correo = updated.Correo;
        cliente.DocumentoIdentidad = updated.DocumentoIdentidad;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente is null) return false;

        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
        return true;
    }
}