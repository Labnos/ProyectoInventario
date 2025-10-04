using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Services;

namespace ProyectoInventario.Endpoints;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/reportes").WithTags("Reportes");

        // Endpoint para obtener productos con bajo stock
        // Corresponde a REQ5 y REQ14 para las alertas del dashboard
        group.MapGet("/bajo-stock/{umbral:int}", [Authorize] async (
            int umbral,
            ReportService service) =>
        {
            var productos = await service.GetProductosBajoStockAsync(umbral);
            return Results.Ok(productos);
        });

        // Endpoint para obtener el total de ventas en una fecha específica
        // Útil para el REQ16 (Ingresos diarios)
        group.MapGet("/ventas-por-fecha/{fecha:datetime}", [Authorize] async (
            DateTime fecha,
            ReportService service) =>
        {
            var total = await service.GetTotalVentasPorFechaAsync(fecha);
            return Results.Ok(new { fecha = fecha.Date, totalVentas = total });
        });

        // Endpoint para obtener los ingresos desglosados por sucursal
        // Corresponde al REQ7 (desempeño por sucursal)
        group.MapGet("/ingresos-por-sucursal", [Authorize] async (ReportService service) =>
        {
            var ingresos = await service.GetIngresosPorSucursalAsync();
            return Results.Ok(ingresos);
        });

        // Endpoint para obtener los productos más vendidos
        // Corresponde al REQ7
        group.MapGet("/productos-mas-vendidos", [Authorize] async (
            [FromQuery] int? top,
            ReportService service) =>
        {
            var productos = await service.GetProductosMasVendidosAsync(top ?? 5);
            return Results.Ok(productos);
        });

        // Endpoint para obtener el historial de ventas entre dos fechas
        // Corresponde al REQ7 (ventas mensuales)
        group.MapGet("/ventas-entre-fechas", [Authorize] async (
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fin,
            ReportService service) =>
        {
            var ventas = await service.GetVentasEntreFechasAsync(inicio, fin);
            return Results.Ok(ventas);
        });
    }
}