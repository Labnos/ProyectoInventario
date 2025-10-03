using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Endpoints;

public static class EnvioEndpoints
{
    public static void MapEnvioEndpoints(this WebApplication app)
    {
        //  Obtener todos los envíos
        app.MapGet("/api/envios", [Authorize] async (AppDbContext db) =>
            await db.Envios.Include(e => e.Venta).ToListAsync());

        // Obtener envío por ID
        app.MapGet("/api/envios/{id}", [Authorize] async (int id, AppDbContext db) =>
        {
            var envio = await db.Envios.Include(e => e.Venta).FirstOrDefaultAsync(e => e.Id == id);
            return envio is not null ? Results.Ok(envio) : Results.NotFound();
        });

        //  Crear nuevo envío
        app.MapPost("/api/envios", [Authorize] async ([FromBody] Envio envio, AppDbContext db) =>
        {
            var venta = await db.Ventas.FindAsync(envio.VentaId);
            if (venta is null) return Results.BadRequest("La venta asociada no existe.");

            db.Envios.Add(envio);
            await db.SaveChangesAsync();
            return Results.Created($"/api/envios/{envio.Id}", envio);
        });

        //  Actualizar envío
        app.MapPut("/api/envios/{id}", [Authorize] async (int id, [FromBody] Envio updated, AppDbContext db) =>
        {
            var existing = await db.Envios.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Estado = updated.Estado;
            existing.Medio = updated.Medio;
            existing.Guia = updated.Guia;
            existing.FechaEnvio = updated.FechaEnvio;
            existing.Destinatario = updated.Destinatario;
            existing.DireccionEntrega = updated.DireccionEntrega;
            existing.TelefonoContacto = updated.TelefonoContacto;
            existing.Observaciones = updated.Observaciones;

            await db.SaveChangesAsync();
            return Results.Ok(existing);
        });

        //  Eliminar envío
        app.MapDelete("/api/envios/{id}", [Authorize(Policy = "AdminOnly")] async (int id, AppDbContext db) =>
        {
            var envio = await db.Envios.FindAsync(id);
            if (envio is null) return Results.NotFound();

            db.Envios.Remove(envio);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}