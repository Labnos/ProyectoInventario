using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class VentaService
{
    private readonly AppDbContext _db;

    public VentaService(AppDbContext db)
    {
        _db = db;
    }

    // 🔍 Obtener todas las ventas
    public async Task<List<Venta>> GetAllAsync()
    {
        return await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .ToListAsync();
    }

    // 🔍 Obtener venta por ID
    public async Task<Venta?> GetByIdAsync(int id)
    {
        return await _db.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    // 📆 Obtener ventas entre fechas
    public async Task<List<Venta>> GetByFechaAsync(DateTime inicio, DateTime fin)
    {
        return await _db.Ventas
            .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
            .ToListAsync();
    }

    // ➕ Registrar nueva venta
    public async Task<Venta?> CreateAsync(Venta venta)
    {
        var cliente = await _db.Clientes.FindAsync(venta.ClienteId);
        if (cliente is null) return null;

        decimal total = 0;
        foreach (var detalle in venta.Detalles)
        {
            var producto = await _db.Productos.FindAsync(detalle.ProductoId);
            if (producto is null || producto.Stock < detalle.Cantidad)
                return null;

            detalle.PrecioUnitario = producto.PrecioVendedor;
            total += detalle.PrecioUnitario * detalle.Cantidad;

            producto.Stock -= detalle.Cantidad;
        }

        venta.Total = total;
        venta.Fecha = DateTime.UtcNow;

        _db.Ventas.Add(venta);
        await _db.SaveChangesAsync();
        return venta;
    }

    // ❌ Eliminar venta
    public async Task<bool> DeleteAsync(int id)
    {
        var venta = await _db.Ventas.FindAsync(id);
        if (venta is null) return false;

        _db.Ventas.Remove(venta);
        await _db.SaveChangesAsync();
        return true;
    }

    // 🔍 Obtener detalles por ID de venta
    public async Task<List<VentaDetalle>> GetDetallesByVentaIdAsync(int ventaId)
    {
        return await _db.VentaDetalles
            .Where(d => d.VentaId == ventaId)
            .Include(d => d.Producto)
            .ToListAsync();
    }

    // 🔍 Obtener detalles por producto vendido
    public async Task<List<VentaDetalle>> GetDetallesByProductoIdAsync(int productoId)
    {
        return await _db.VentaDetalles
            .Where(d => d.ProductoId == productoId)
            .Include(d => d.Venta)
            .ThenInclude(v => v.Cliente)
            .ToListAsync();
    }
}