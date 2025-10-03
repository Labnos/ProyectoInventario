using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Endpoints;

public static class VentaEndpoints
{
    public static void MapVentaEndpoints(this WebApplication app)
    {
        //  Obtener todas las ventas
        app.MapGet("/api/ventas", [Authorize] async (AppDbContext db) =>
            await db.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .ToListAsync());

        //  Obtener venta por ID
        app.MapGet("/api/ventas/{id}", [Authorize] async (int id, AppDbContext db) =>
        {
            var venta = await db.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            return venta is not null ? Results.Ok(venta) : Results.NotFound();
        });

        //  Registrar nueva venta con detalles
        app.MapPost("/api/ventas", [Authorize] async ([FromBody] Venta venta, AppDbContext db) =>
        {
            var cliente = await db.Clientes.FindAsync(venta.ClienteId);
            if (cliente is null) return Results.BadRequest("Cliente no válido.");

            // Validar productos y calcular total
            decimal total = 0;
            foreach (var detalle in venta.Detalles)
            {
                var producto = await db.Productos.FindAsync(detalle.ProductoId);
                if (producto is null) return Results.BadRequest($"Producto ID {detalle.ProductoId} no existe.");

                detalle.PrecioUnitario = producto.PrecioVendedor;
                total += detalle.PrecioUnitario * detalle.Cantidad;

                // Opcional: actualizar stock
                producto.Stock -= detalle.Cantidad;
            }

            venta.Total = total;
            venta.Fecha = DateTime.UtcNow;

            db.Ventas.Add(venta);
            await db.SaveChangesAsync();
            return Results.Created($"/api/ventas/{venta.Id}", venta);
        });

        //  Obtener ventas entre fechas
        app.MapGet("/api/ventas/entre-fechas", [Authorize] async (
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin,
            AppDbContext db) =>
        {
            var ventas = await db.Ventas
                .Where(v => v.Fecha >= inicio && v.Fecha <= fin)
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .ToListAsync();

            return Results.Ok(ventas);
        });

        //  Eliminar venta
        app.MapDelete("/api/ventas/{id}", [Authorize(Policy = "AdminOnly")] async (int id, AppDbContext db) =>
        {
            var venta = await db.Ventas.FindAsync(id);
            if (venta is null) return Results.NotFound();

            db.Ventas.Remove(venta);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}