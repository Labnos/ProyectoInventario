// En: Validators/SucursalValidator.cs
using FluentValidation;
using ProyectoInventario.Models;

public class SucursalValidator : AbstractValidator<Sucursal>
{
    public SucursalValidator()
    {
        RuleFor(s => s.Nombre)
            .NotEmpty().WithMessage("El nombre de la sucursal es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede tener más de 150 caracteres.");

        RuleFor(s => s.Direccion)
            .NotEmpty().WithMessage("La dirección es obligatoria.");
    }
}