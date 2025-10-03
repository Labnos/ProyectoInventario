using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class ProveedorService
{
    private readonly AppDbContext _db;

    public ProveedorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Proveedor>> GetAllAsync() =>
        await _db.Proveedores.ToListAsync();

    public async Task<Proveedor?> GetByIdAsync(int id) =>
        await _db.Proveedores.FindAsync(id);

    public async Task<Proveedor> CreateAsync(Proveedor proveedor)
    {
        _db.Proveedores.Add(proveedor);
        await _db.SaveChangesAsync();
        return proveedor;
    }

    public async Task<bool> UpdateAsync(int id, Proveedor updated)
    {
        var proveedor = await _db.Proveedores.FindAsync(id);
        if (proveedor is null) return false;

        proveedor.Nombre = updated.Nombre;
        proveedor.Telefono = updated.Telefono;
        proveedor.Direccion = updated.Direccion;
        proveedor.Correo = updated.Correo;
        proveedor.Nit = updated.Nit;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var proveedor = await _db.Proveedores.FindAsync(id);
        if (proveedor is null) return false;

        _db.Proveedores.Remove(proveedor);
        await _db.SaveChangesAsync();
        return true;
    }
}