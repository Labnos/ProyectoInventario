using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class ProductoService
{
    private readonly AppDbContext _db;

    public ProductoService(AppDbContext db)
    {
        _db = db;
    }

    // 🔍 Obtener todos los productos
    public async Task<List<Producto>> GetAllAsync()
    {
        return await _db.Productos.ToListAsync();
    }

    // 🔍 Obtener producto por ID
    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _db.Productos.FindAsync(id);
    }

    // ➕ Crear nuevo producto
    public async Task<Producto> CreateAsync(Producto producto)
    {
        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();
        return producto;
    }

    // Actualizar producto existente
    public async Task<bool> UpdateAsync(int id, Producto updated)
    {
        var existing = await _db.Productos.FindAsync(id);
        if (existing is null) return false;

        existing.Nombre = updated.Nombre;
        existing.Tipo = updated.Tipo;
        existing.Talla = updated.Talla;
        existing.Color = updated.Color;
        existing.PrecioEntrada = updated.PrecioEntrada;
        existing.PrecioVendedor = updated.PrecioVendedor;
        existing.Sucursal = updated.Sucursal;

        await _db.SaveChangesAsync();
        return true;
    }

    //  Eliminar producto
    public async Task<bool> DeleteAsync(int id)
    {
        var producto = await _db.Productos.FindAsync(id);
        if (producto is null) return false;

        _db.Productos.Remove(producto);
        await _db.SaveChangesAsync();
        return true;
    }

    //  Obtener productos con bajo stock (si agregas campo Stock)
    public async Task<List<Producto>> GetProductosBajoStockAsync(int umbral)
    {
        return await _db.Productos
            .Where(p => p.Stock < umbral)
            .ToListAsync();
    }
}