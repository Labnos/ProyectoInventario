using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class ReportService
{
    private readonly AppDbContext _db;

    public ReportService(AppDbContext db)
    {
        _db = db;
    }

    //  Productos con bajo stock
    public async Task<List<Producto>> GetProductosBajoStockAsync(int umbral)
    {
        return await _db.Productos
            .Where(p => p.Stock < umbral)
            .ToListAsync();
    }

    //  Total de ventas por fecha
    public async Task<decimal> GetTotalVentasPorFechaAsync(DateTime fecha)
    {
        return await _db.Ventas
            .Where(v => v.Fecha.Date == fecha.Date)
            .SumAsync(v => v.Total);
    }

    //  Ingresos por sucursal
    public async Task<Dictionary<string, decimal>> GetIngresosPorSucursalAsync()
    {
        return await _db.Ventas
            .GroupBy(v => v.Sucursal)
            .Select(g => new { Sucursal = g.Key, Total = g.Sum(v => v.Total) })
            .ToDictionaryAsync(x => x.Sucursal, x => x.Total);
    }

    //  Productos más vendidos
    public async Task<List<(string NombreProducto, int TotalVendidos)>> GetProductosMasVendidosAsync(int top = 5)
    {
        return await _db.VentaDetalles
            .GroupBy(vd => vd.ProductoId)
            .Select(g => new {
                ProductoId = g.Key,
                TotalVendidos = g.Sum(vd => vd.Cantidad)
            })
            .OrderByDescending(x => x.TotalVendidos)
            .Take(top)
            .Join(_db.Productos,
                x => x.ProductoId,
                p => p.Id,
                (x, p) => new { p.Nombre, x.TotalVendidos })
            .Select(x => (x.Nombre, x.TotalVendidos))
            .ToListAsync();
    }

    //  Ventas entre fechas
    public async Task<List<Venta>> GetVentasEntreFechasAsync(DateTime inicio, DateTime fin)
    {
        return await _db.Ventas
            .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
            .Include(v => v.Detalles)
            .ToListAsync();
    }
}