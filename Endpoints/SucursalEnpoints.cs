using Microsoft.AspNetCore.Mvc;
using ProyectoInventario.Models;
using ProyectoInventario.Services;
using FluentValidation; // <-- Asegúrate de tener este 'using'
using Microsoft.AspNetCore.Authorization;

namespace ProyectoInventario.Endpoints
{
    public static class SucursalEndpoints
    {
        public static void MapSucursalEndpoints(this WebApplication app)
        {
            var sucursalGroup = app.MapGroup("/api/sucursales").RequireAuthorization();

            sucursalGroup.MapPost("/", async (
                [FromBody] Sucursal sucursal,
                IValidator<Sucursal> validator, // <-- Parámetro que causaba el error
                SucursalService service) =>
            {
                var validationResult = await validator.ValidateAsync(sucursal);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                await service.CreateAsync(sucursal);
                return Results.Created($"/api/sucursales/{sucursal.Id}", sucursal);
            });

            // Aquí puedes añadir los otros endpoints para sucursales (GET, PUT, DELETE)
        }
    }
}