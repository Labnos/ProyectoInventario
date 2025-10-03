using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class SucursalService
{
    private readonly AppDbContext _db;

    public SucursalService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Sucursal>> GetAllAsync() =>
        await _db.Sucursales.ToListAsync();

    public async Task<Sucursal?> GetByIdAsync(int id) =>
        await _db.Sucursales.FindAsync(id);

    public async Task<Sucursal> CreateAsync(Sucursal sucursal)
    {
        _db.Sucursales.Add(sucursal);
        await _db.SaveChangesAsync();
        return sucursal;
    }

    public async Task<bool> UpdateAsync(int id, Sucursal updated)
    {
        var sucursal = await _db.Sucursales.FindAsync(id);
        if (sucursal is null) return false;

        sucursal.Nombre = updated.Nombre;
        sucursal.Direccion = updated.Direccion;
        sucursal.Telefono = updated.Telefono;
        sucursal.Encargado = updated.Encargado;
        sucursal.Codigo = updated.Codigo;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sucursal = await _db.Sucursales.FindAsync(id);
        if (sucursal is null) return false;

        _db.Sucursales.Remove(sucursal);
        await _db.SaveChangesAsync();
        return true;
    }
}