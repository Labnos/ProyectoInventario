using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

// Clase auxiliar para los resultados paginados
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
}

public class ProductoService
{
    private readonly AppDbContext _db;

    public ProductoService(AppDbContext db)
    {
        _db = db;
    }

    // --- MÉTODO MODIFICADO para paginación ---
    public async Task<PagedResult<Producto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.Productos.CountAsync();
        var items = await _db.Productos
                             .OrderBy(p => p.Nombre) // Es buena práctica ordenar los resultados
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

        return new PagedResult<Producto> { Items = items, TotalCount = totalCount };
    }

    // --- MÉTODO CORREGIDO ---
    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _db.Productos.FindAsync(id);
    }

    public async Task<Producto> CreateAsync(Producto producto)
    {
        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();
        return producto;
    }

    public async Task<bool> UpdateAsync(int id, Producto producto)
    {
        var dbProducto = await _db.Productos.FindAsync(id);
        if (dbProducto is null) return false;

        dbProducto.Nombre = producto.Nombre;
        dbProducto.TipoPrenda = producto.TipoPrenda;
        dbProducto.Proveedor = producto.Proveedor;
        dbProducto.Sucursal = producto.Sucursal;
        dbProducto.PrecioAdquisicion = producto.PrecioAdquisicion;
        dbProducto.PrecioVenta = producto.PrecioVenta;
        dbProducto.Stock = producto.Stock;
        
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbProducto = await _db.Productos.FindAsync(id);
        if (dbProducto is null) return false;
        
        _db.Productos.Remove(dbProducto);
        await _db.SaveChangesAsync();
        return true;
    }
}