using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class ProductoValidator : AbstractValidator<Producto>
{
    public ProductoValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(p => p.Tipo)
            .NotEmpty().WithMessage("El tipo de producto es obligatorio.")
            .MaximumLength(50);

        RuleFor(p => p.Talla)
            .NotEmpty().WithMessage("La talla es obligatoria.")
            .MaximumLength(20);

        RuleFor(p => p.Color)
            .NotEmpty().WithMessage("El color es obligatorio.")
            .MaximumLength(30);

        RuleFor(p => p.PrecioEntrada)
            .GreaterThan(0).WithMessage("El precio de entrada debe ser mayor a cero.");

        RuleFor(p => p.PrecioVendedor)
            .GreaterThan(p => p.PrecioEntrada)
            .WithMessage("El precio de venta debe ser mayor al precio de entrada.");

        RuleFor(p => p.Sucursal)
            .NotEmpty().WithMessage("La sucursal es obligatoria.")
            .MaximumLength(50);
    }
}