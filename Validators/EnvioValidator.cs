using FluentValidation;
using ProyectoInventario.Models;

namespace ProyectoInventario.Validators;

public class EnvioValidator : AbstractValidator<Envio>
{
    public EnvioValidator()
    {
        RuleFor(e => e.VentaId)
            .GreaterThan(0).WithMessage("Debe asociarse a una venta válida.");

        RuleFor(e => e.Estado)
            .NotEmpty().MaximumLength(30);

        RuleFor(e => e.Medio)
            .NotEmpty().MaximumLength(50);

        RuleFor(e => e.Guia)
            .MaximumLength(100);

        RuleFor(e => e.Destinatario)
            .MaximumLength(100);

        RuleFor(e => e.DireccionEntrega)
            .MaximumLength(200);

        RuleFor(e => e.TelefonoContacto)
            .MaximumLength(20);

        RuleFor(e => e.Observaciones)
            .MaximumLength(100);
    }
}