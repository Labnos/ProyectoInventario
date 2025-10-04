using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Data;
using ProyectoInventario.Models;

namespace ProyectoInventario.Services;

public class PromocionService
{
    private readonly AppDbContext _db;

    public PromocionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Promocion>> GetAllAsync() => await _db.Promociones.ToListAsync();

    public async Task<Promocion?> GetByIdAsync(int id) => await _db.Promociones.FindAsync(id);

    public async Task<Promocion> CreateAsync(Promocion promocion)
    {
        _db.Promociones.Add(promocion);
        await _db.SaveChangesAsync();
        return promocion;
    }

    public async Task<bool> UpdateAsync(int id, Promocion promocion)
    {
        var dbPromocion = await _db.Promociones.FindAsync(id);
        if (dbPromocion is null) return false;

        dbPromocion.Nombre = promocion.Nombre;
        dbPromocion.Descripcion = promocion.Descripcion;
        dbPromocion.PorcentajeDescuento = promocion.PorcentajeDescuento;
        dbPromocion.FechaInicio = promocion.FechaInicio;
        dbPromocion.FechaFin = promocion.FechaFin;
        dbPromocion.Activa = promocion.Activa;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbPromocion = await _db.Promociones.FindAsync(id);
        if (dbPromocion is null) return false;

        _db.Promociones.Remove(dbPromocion);
        await _db.SaveChangesAsync();
        return true;
    }
}