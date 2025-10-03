using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class VentaDetalleValidator : AbstractValidator<VentaDetalle>
{
    public VentaDetalleValidator()
    {
        RuleFor(d => d.ProductoId)
            .GreaterThan(0).WithMessage("Debe asociarse a un producto válido.");

        RuleFor(d => d.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

        RuleFor(d => d.PrecioUnitario)
            .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero.");
    }
}