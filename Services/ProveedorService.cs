using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;
using ProyectoInventario.Services; // Asegúrate que PagedResult esté accesible

namespace ProyectoInventario.Services;

public class ProveedorService
{
    private readonly AppDbContext _db;

    public ProveedorService(AppDbContext db)
    {
        _db = db;
    }

    // --- MÉTODO MODIFICADO para paginación ---
    public async Task<PagedResult<Proveedor>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.Proveedores.CountAsync();
        var items = await _db.Proveedores
                             .OrderBy(p => p.NombreEmpresa)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

        return new PagedResult<Proveedor> { Items = items, TotalCount = totalCount };
    }

    public async Task<Proveedor?> GetByIdAsync(int id) => await _db.Proveedores.FindAsync(id);

    public async Task<Proveedor> CreateAsync(Proveedor proveedor)
    {
        _db.Proveedores.Add(proveedor);
        await _db.SaveChangesAsync();
        return proveedor;
    }

    public async Task<bool> UpdateAsync(int id, Proveedor proveedor)
    {
        var dbProveedor = await _db.Proveedores.FindAsync(id);
        if (dbProveedor is null) return false;

        dbProveedor.NombreEmpresa = proveedor.NombreEmpresa;
        dbProveedor.NombreContacto = proveedor.NombreContacto;
        dbProveedor.Telefono = proveedor.Telefono;
        dbProveedor.Direccion = proveedor.Direccion;
        
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbProveedor = await _db.Proveedores.FindAsync(id);
        if (dbProveedor is null) return false;

        _db.Proveedores.Remove(dbProveedor);
        await _db.SaveChangesAsync();
        return true;
    }
}