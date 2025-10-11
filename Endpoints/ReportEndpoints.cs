using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProyectoInventario.Services;
using System;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/reportes").WithTags("Reportes");

        group.MapGet("/ventas-entre-fechas", [Authorize] async (
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin,
            ReportService service) =>
        {
            var ventas = await service.GetVentasEntreFechasAsync(inicio, fin);
            return Results.Ok(ventas);
        });

        group.MapGet("/productos-mas-vendidos", [Authorize] async (ReportService service) =>
        {
            var productos = await service.GetProductosMasVendidosAsync();
            return Results.Ok(productos);
        });

        group.MapGet("/bajo-stock/{umbral}", [Authorize] async (int umbral, ReportService service) =>
        {
            var productos = await service.GetProductosBajoStockAsync(umbral);
            return Results.Ok(productos);
        });

        group.MapGet("/ganancia-por-producto", [Authorize] async (
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin,
            ReportService service) =>
        {
            var ganancias = await service.GetGananciaPorProductoAsync(inicio, fin);
            return Results.Ok(ganancias);
        });

        group.MapGet("/inventario-actual", [Authorize] async (
            [FromQuery] string? sucursal,
            ReportService service) =>
        {
            var inventario = await service.GetInventarioActualAsync(sucursal);
            return Results.Ok(inventario);
        });
    }
}