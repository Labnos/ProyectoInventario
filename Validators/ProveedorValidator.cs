using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class ProveedorValidator : AbstractValidator<Proveedor>
{
    public ProveedorValidator()
    {
        RuleFor(p => p.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Telefono).MaximumLength(20);
        RuleFor(p => p.Direccion).MaximumLength(200);
        RuleFor(p => p.Correo).EmailAddress().When(p => !string.IsNullOrWhiteSpace(p.Correo)).MaximumLength(100);
        RuleFor(p => p.Nit).MaximumLength(50);
        RuleFor(p => p.NombreEmpresa).NotEmpty().MaximumLength(100);
        RuleFor(p => p.NombreContacto).NotEmpty().MaximumLength(100);
    }
}