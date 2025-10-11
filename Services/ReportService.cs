using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInventario.Services
{
    public class ReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Venta>> GetVentasEntreFechasAsync(DateTime inicio, DateTime fin)
        {
            return await _db.Ventas
                .Where(v => v.Fecha >= inicio && v.Fecha <= fin.AddDays(1)) // Se agrega AddDays(1) para incluir todo el día de la fecha fin
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task<List<(string NombreProducto, int TotalVendidos)>> GetProductosMasVendidosAsync(int top = 5)
        {
            // 1. Ejecuta la consulta y trae los resultados a la memoria como una lista de objetos anónimos.
            var results = await _db.VentaDetalles
                .GroupBy(vd => vd.Producto.Nombre)
                .Select(g => new {
                    NombreProducto = g.Key,
                    TotalVendidos = g.Sum(vd => vd.Cantidad)
                })
                .OrderByDescending(x => x.TotalVendidos)
                .Take(top)
                .ToListAsync();

            // 2. Una vez en memoria, convierte la lista de objetos a una lista de tuplas.
            return results.Select(x => (x.NombreProducto, x.TotalVendidos)).ToList();
        }

        public async Task<List<Producto>> GetProductosBajoStockAsync(int umbral)
        {
            return await _db.Productos
                .Where(p => p.Stock < umbral)
                .ToListAsync();
        }

        public async Task<List<object>> GetGananciaPorProductoAsync(DateTime inicio, DateTime fin)
        {
            return await _db.VentaDetalles
                .Where(d => d.Venta.Fecha >= inicio && d.Venta.Fecha <= fin.AddDays(1))
                .Include(d => d.Producto)
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new {
                    Producto = g.Key,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    GananciaTotal = g.Sum(d => (d.PrecioUnitario - d.Producto.PrecioEntrada) * d.Cantidad)
                })
                .OrderByDescending(x => x.GananciaTotal)
                .ToListAsync<object>();
        }

        public async Task<List<Producto>> GetInventarioActualAsync(string? sucursal = null)
        {
            var query = _db.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(sucursal))
            {
                query = query.Where(p => p.Sucursal == sucursal);
            }

            return await query.OrderBy(p => p.Nombre).ToListAsync();
        }
    }
}