using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class VentaValidator : AbstractValidator<Venta>
{
    public VentaValidator()
    {
        RuleFor(v => v.ClienteId)
            .GreaterThan(0).WithMessage("Debe asociarse a un cliente válido.");

        RuleFor(v => v.Total)
            .GreaterThan(0).WithMessage("El total debe ser mayor a cero.");

        RuleFor(v => v.Sucursal)
            .MaximumLength(50);

        RuleFor(v => v.Estado)
            .MaximumLength(30);

        RuleFor(v => v.Detalles)
            .NotEmpty().WithMessage("La venta debe tener al menos un producto.");
    }
}